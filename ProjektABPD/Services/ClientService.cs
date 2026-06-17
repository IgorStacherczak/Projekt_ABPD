using Microsoft.EntityFrameworkCore;
using ProjektABPD.Data;
using ProjektABPD.DTOs;
using ProjektABPD.Exceptions;
using ProjektABPD.Models;

namespace ProjektABPD.Services;

public class ClientService : IClientService
{
    private readonly DatabaseContext _context;

    public ClientService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<List<ClientDto>> GetClientsAsync()
    {
        return await _context.Clients
            .Where(c => !c.IsDeleted)
            .Select(c => new ClientDto
            {
                IdClient = c.IdClient,
                ClientType = c.ClientType,
                FirstName = c.FirstName,
                LastName = c.LastName,
                CompanyName = c.CompanyName,
                Address = c.Address,
                Email = c.Email,
                PhoneNumber = c.PhoneNumber,
                Pesel = c.Pesel,
                Krs = c.Krs,
                IsDeleted = c.IsDeleted
            })
            .ToListAsync();
    }

    public async Task<ClientDto> CreateClientAsync(CreateClientDto dto)
    {
        if (dto.ClientType == "Individual")
        {
            bool peselExists = await _context.Clients
                .AnyAsync(c => c.Pesel == dto.Pesel && !c.IsDeleted);

            if (peselExists)
                throw new BadRequestException("Client with this PESEL already exists.");
        }

        if (dto.ClientType == "Company")
        {
            bool krsExists = await _context.Clients
                .AnyAsync(c => c.Krs == dto.Krs && !c.IsDeleted);

            if (krsExists)
                throw new BadRequestException("Client with this KRS already exists.");
        }

        var client = new Client
        {
            ClientType = dto.ClientType,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            CompanyName = dto.CompanyName,
            Address = dto.Address,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            Pesel = dto.Pesel,
            Krs = dto.Krs,
            IsDeleted = false
        };

        _context.Clients.Add(client);
        await _context.SaveChangesAsync();

        return new ClientDto
        {
            IdClient = client.IdClient,
            ClientType = client.ClientType,
            FirstName = client.FirstName,
            LastName = client.LastName,
            CompanyName = client.CompanyName,
            Address = client.Address,
            Email = client.Email,
            PhoneNumber = client.PhoneNumber,
            Pesel = client.Pesel,
            Krs = client.Krs,
            IsDeleted = client.IsDeleted
        };
    }

    public async Task DeleteClientAsync(int id)
    {
        var client = await _context.Clients.FindAsync(id);

        if (client == null || client.IsDeleted)
            throw new NotFoundException("Client not found.");

        client.IsDeleted = true;
        await _context.SaveChangesAsync();
    }
    
    public async Task<ClientDto> UpdateClientAsync(int id, UpdateClientDto dto)
    {
        var client = await _context.Clients
            .FirstOrDefaultAsync(c => c.IdClient == id && !c.IsDeleted);

        if (client == null)
        {
            throw new NotFoundException("Client not found.");
        }

        client.FirstName = dto.FirstName;
        client.LastName = dto.LastName;
        client.CompanyName = dto.CompanyName;

        client.Address = dto.Address;
        client.Email = dto.Email;
        client.PhoneNumber = dto.PhoneNumber;

        await _context.SaveChangesAsync();

        return new ClientDto
        {
            IdClient = client.IdClient,
            ClientType = client.ClientType,
            FirstName = client.FirstName,
            LastName = client.LastName,
            CompanyName = client.CompanyName,
            Address = client.Address,
            Email = client.Email,
            PhoneNumber = client.PhoneNumber,
            Pesel = client.Pesel,
            Krs = client.Krs,
            IsDeleted = client.IsDeleted
        };
    }
}