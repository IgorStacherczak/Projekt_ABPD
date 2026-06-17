using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjektABPD.DTOs;
using ProjektABPD.Exceptions;
using ProjektABPD.Services;

namespace ProjektABPD.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DiscountsController : ControllerBase
{
    private readonly IDiscountService _discountService;

    public DiscountsController(IDiscountService discountService)
    {
        _discountService = discountService;
    }

    [HttpGet]
    public async Task<IActionResult> GetDiscounts()
    {
        var discounts = await _discountService.GetDiscountsAsync();
        return Ok(discounts);
    }

    [HttpPost]
    public async Task<IActionResult> CreateDiscount(CreateDiscountDto dto)
    {
        try
        {
            var discount = await _discountService.CreateDiscountAsync(dto);
            return Created("", discount);
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
    }
}