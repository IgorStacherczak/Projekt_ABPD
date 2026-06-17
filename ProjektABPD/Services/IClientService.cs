using ProjektABPD.DTOs;

namespace ProjektABPD.Services;

public interface IClientService
{
    Task<List<ClientDto>> GetClientsAsync();

    Task<ClientDto> CreateClientAsync(CreateClientDto dto);

    Task DeleteClientAsync(int id);

    Task<ClientDto> UpdateClientAsync(int id, UpdateClientDto dto);
}