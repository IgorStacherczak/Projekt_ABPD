using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjektABPD.Exceptions;
using ProjektABPD.Services;

namespace ProjektABPD.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RevenuesController : ControllerBase
{
    private readonly IRevenueService _revenueService;

    public RevenuesController(IRevenueService revenueService)
    {
        _revenueService = revenueService;
    }

    [HttpGet("current")]
    public async Task<IActionResult> GetCurrentRevenue(int? idSoftware, string currency = "PLN")
    {
        try
        {
            var revenue = await _revenueService.GetCurrentRevenueAsync(idSoftware, currency);
            return Ok(revenue);
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("predicted")]
    public async Task<IActionResult> GetPredictedRevenue(int? idSoftware, string currency = "PLN")
    {
        try
        {
            var revenue = await _revenueService.GetPredictedRevenueAsync(idSoftware, currency);
            return Ok(revenue);
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
    }
}