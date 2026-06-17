using Microsoft.EntityFrameworkCore;
using ProjektABPD.Data;
using ProjektABPD.DTOs;
using ProjektABPD.Exceptions;
using ProjektABPD.Models;

namespace ProjektABPD.Services;

public class ContractService : IContractService
{
    private readonly DatabaseContext _context;

    public ContractService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<ContractDto> CreateContractAsync(CreateContractDto dto)
    {
        var client = await _context.Clients.FindAsync(dto.IdClient);

        if (client == null || client.IsDeleted)
            throw new NotFoundException("Client not found");

        var software = await _context.Softwares.FindAsync(dto.IdSoftware);

        if (software == null)
            throw new NotFoundException("Software not found");

        var softwareVersion = await _context.SoftwareVersions
            .FirstOrDefaultAsync(v => v.IdSoftwareVersion == dto.IdSoftwareVersion 
                                      && v.IdSoftware == dto.IdSoftware);

        if (softwareVersion == null)
            throw new NotFoundException("Software version not found");

        if ((dto.EndDate - dto.StartDate).TotalDays < 3 || (dto.EndDate - dto.StartDate).TotalDays > 30)
            throw new BadRequestException("Contract duration must be between 3 and 30 days");

        if (dto.SupportYears < 1 || dto.SupportYears > 3)
            throw new BadRequestException("Support years must be between 1 and 3");

        var hasActiveContract = await _context.Contracts.AnyAsync(c =>
            c.IdClient == dto.IdClient &&
            c.IdSoftware == dto.IdSoftware &&
            !c.IsCancelled &&
            !c.IsSigned);

        if (hasActiveContract)
            throw new BadRequestException("Client already has an active contract for this software");

        var bestDiscount = await _context.Discounts
            .Where(d => d.StartDate <= dto.StartDate && d.EndDate >= dto.StartDate)
            .MaxAsync(d => (int?)d.Percentage) ?? 0;

        var isReturningClient = await _context.Contracts.AnyAsync(c =>
            c.IdClient == dto.IdClient &&
            c.IsSigned);

        var totalDiscount = bestDiscount;

        if (isReturningClient)
            totalDiscount += 5;

        var finalPrice = dto.Price + ((dto.SupportYears - 1) * 1000);
        finalPrice = finalPrice - (finalPrice * totalDiscount / 100);

        var contract = new Contract
        {
            IdClient = dto.IdClient,
            IdSoftware = dto.IdSoftware,
            IdSoftwareVersion = dto.IdSoftwareVersion,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            Price = finalPrice,
            SupportYears = dto.SupportYears,
            IsSigned = false,
            IsCancelled = false
        };

        _context.Contracts.Add(contract);
        await _context.SaveChangesAsync();

        return new ContractDto
        {
            IdContract = contract.IdContract,
            IdClient = contract.IdClient,
            IdSoftware = contract.IdSoftware,
            IdSoftwareVersion = contract.IdSoftwareVersion,
            StartDate = contract.StartDate,
            EndDate = contract.EndDate,
            Price = contract.Price,
            SupportYears = contract.SupportYears,
            IsSigned = contract.IsSigned,
            IsCancelled = contract.IsCancelled
        };
    }

    public async Task SignContractAsync(int id)
    {
        var contract = await _context.Contracts.FindAsync(id);

        if (contract == null)
            throw new NotFoundException("Contract not found");

        if (contract.IsCancelled)
            throw new BadRequestException("Contract is cancelled");

        if (contract.IsSigned)
            throw new BadRequestException("Contract is already signed");

        contract.IsSigned = true;

        await _context.SaveChangesAsync();
    }
}