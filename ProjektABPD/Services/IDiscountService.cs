using ProjektABPD.DTOs;

namespace ProjektABPD.Services;

public interface IDiscountService
{
    Task<List<DiscountDto>> GetDiscountsAsync();
    Task<DiscountDto> CreateDiscountAsync(CreateDiscountDto dto);
}