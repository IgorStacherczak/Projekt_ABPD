using ProjektABPD.DTOs;

namespace ProjektABPD.Services;

public interface IPaymentService
{
    Task<PaymentDto> CreatePaymentAsync(CreatePaymentDto dto);
}