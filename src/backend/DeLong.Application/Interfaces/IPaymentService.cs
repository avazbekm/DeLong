using DeLong.Service.DTOs.Payments;

namespace DeLong.Service.Interfaces;

public interface IPaymentService
{
    ValueTask<PaymentResultDto> AddAsync(PaymentCreationDto dto);
    ValueTask<IEnumerable<PaymentResultDto>> RetrieveBySaleIdAsync(long saleId);
    ValueTask<IEnumerable<PaymentResultDto>> RetrieveAllAsync();
}