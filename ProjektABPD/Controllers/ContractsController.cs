using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjektABPD.DTOs;
using ProjektABPD.Exceptions;
using ProjektABPD.Services;

namespace ProjektABPD.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ContractsController : ControllerBase
{
    private readonly IContractService _contractService;

    public ContractsController(IContractService contractService)
    {
        _contractService = contractService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateContract(CreateContractDto dto)
    {
        try
        {
            var contract = await _contractService.CreateContractAsync(dto);
            return Created($"api/contracts/{contract.IdContract}", contract);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("{id}/sign")]
    public async Task<IActionResult> SignContract(int id)
    {
        try
        {
            await _contractService.SignContractAsync(id);
            return Ok();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
    }
}