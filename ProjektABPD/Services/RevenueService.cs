using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using ProjektABPD.Data;
using ProjektABPD.DTOs;
using ProjektABPD.Exceptions;

namespace ProjektABPD.Services;

public class RevenueService : IRevenueService
{
    private readonly DatabaseContext _context;
    private readonly HttpClient _httpClient;

    public RevenueService(DatabaseContext context, HttpClient httpClient)
    {
        _context = context;
        _httpClient = httpClient;
    }

    public async Task<RevenueDto> GetCurrentRevenueAsync(int? idSoftware, string currency)
    {
        var query = _context.Contracts
            .Where(c => c.IsSigned && !c.IsCancelled);

        if (idSoftware.HasValue)
            query = query.Where(c => c.IdSoftware == idSoftware.Value);

        var amountPln = await query.SumAsync(c => c.Price);
        var converted = await ConvertFromPlnAsync(amountPln, currency);

        return new RevenueDto
        {
            Amount = converted,
            Currency = currency.ToUpper()
        };
    }

    public async Task<RevenueDto> GetPredictedRevenueAsync(int? idSoftware, string currency)
    {
        var query = _context.Contracts
            .Where(c => !c.IsCancelled);

        if (idSoftware.HasValue)
            query = query.Where(c => c.IdSoftware == idSoftware.Value);

        var amountPln = await query.SumAsync(c => c.Price);
        var converted = await ConvertFromPlnAsync(amountPln, currency);

        return new RevenueDto
        {
            Amount = converted,
            Currency = currency.ToUpper()
        };
    }

    private async Task<decimal> ConvertFromPlnAsync(decimal amount, string currency)
    {
        currency = currency.ToUpper();

        if (currency == "PLN")
            return amount;

        var response = await _httpClient.GetAsync($"https://api.nbp.pl/api/exchangerates/rates/a/{currency}/?format=json");

        if (!response.IsSuccessStatusCode)
            throw new BadRequestException("Invalid currency.");

        var json = await response.Content.ReadAsStringAsync();
        using var document = JsonDocument.Parse(json);

        var rate = document.RootElement
            .GetProperty("rates")[0]
            .GetProperty("mid")
            .GetDecimal();

        return Math.Round(amount / rate, 2);
    }
}