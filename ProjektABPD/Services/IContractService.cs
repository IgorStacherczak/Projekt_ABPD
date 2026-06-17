using ProjektABPD.DTOs;

namespace ProjektABPD.Services;

public interface IContractService
{
    Task<ContractDto> CreateContractAsync(CreateContractDto dto);

    Task SignContractAsync(int id);
}