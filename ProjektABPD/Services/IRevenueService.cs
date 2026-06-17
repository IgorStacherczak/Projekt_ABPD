using ProjektABPD.DTOs;

namespace ProjektABPD.Services;

public interface IRevenueService
{
    Task<RevenueDto> GetCurrentRevenueAsync(int? idSoftware, string currency);
    Task<RevenueDto> GetPredictedRevenueAsync(int? idSoftware, string currency);
}