using Microsoft.EntityFrameworkCore;
using ProjektABPD.Data;
using ProjektABPD.DTOs;
using ProjektABPD.Exceptions;
using ProjektABPD.Models;

namespace ProjektABPD.Services;

public class PaymentService : IPaymentService
{
    private readonly DatabaseContext _context;

    public PaymentService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<PaymentDto> CreatePaymentAsync(CreatePaymentDto dto)
    {
        var contract = await _context.Contracts
            .Include(c => c.Payments)
            .FirstOrDefaultAsync(c => c.IdContract == dto.IdContract);

        if (contract == null)
            throw new NotFoundException("Contract not found.");

        if (contract.IsCancelled)
            throw new BadRequestException("Contract is cancelled.");

        if (contract.IsSigned)
            throw new BadRequestException("Contract is already signed.");

        if (DateTime.Now > contract.EndDate)
            throw new BadRequestException("Payment date is after contract end date.");

        if (dto.Amount <= 0)
            throw new BadRequestException("Payment amount must be greater than 0.");

        decimal currentPaid = contract.Payments.Sum(p => p.Amount);
        decimal totalAfterPayment = currentPaid + dto.Amount;

        if (totalAfterPayment > contract.Price)
            throw new BadRequestException("Payment amount exceeds contract price.");

        var payment = new Payment
        {
            IdContract = dto.IdContract,
            Amount = dto.Amount,
            PaymentDate = DateTime.Now
        };

        _context.Payments.Add(payment);

        if (totalAfterPayment == contract.Price)
        {
            contract.IsSigned = true;
        }

        await _context.SaveChangesAsync();

        return new PaymentDto
        {
            IdPayment = payment.IdPayment,
            IdContract = payment.IdContract,
            Amount = payment.Amount,
            PaymentDate = payment.PaymentDate
        };
    }
}