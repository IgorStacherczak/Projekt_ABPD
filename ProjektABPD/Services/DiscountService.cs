using Microsoft.EntityFrameworkCore;
using ProjektABPD.Data;
using ProjektABPD.DTOs;
using ProjektABPD.Exceptions;
using ProjektABPD.Models;

namespace ProjektABPD.Services;

public class DiscountService : IDiscountService
{
    private readonly DatabaseContext _context;

    public DiscountService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<List<DiscountDto>> GetDiscountsAsync()
    {
        return await _context.Discounts
            .Select(d => new DiscountDto
            {
                IdDiscount = d.IdDiscount,
                Name = d.Name,
                Value = d.Percentage,
                StartDate = d.StartDate,
                EndDate = d.EndDate
            })
            .ToListAsync();
    }

    public async Task<DiscountDto> CreateDiscountAsync(CreateDiscountDto dto)
    {
        if (dto.Percentage <= 0 || dto.Percentage > 100)
            throw new BadRequestException("Discount percentage must be between 1 and 100.");

        if (dto.StartDate > dto.EndDate)
            throw new BadRequestException("Start date cannot be later than end date.");

        var discount = new Discount
        {
            Name = dto.Name,
            Percentage = dto.Percentage,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate
        };

        _context.Discounts.Add(discount);
        await _context.SaveChangesAsync();

        return new DiscountDto
        {
            IdDiscount = discount.IdDiscount,
            Name = discount.Name,
            Value = discount.Percentage,
            StartDate = discount.StartDate,
            EndDate = discount.EndDate
        };
    }
}