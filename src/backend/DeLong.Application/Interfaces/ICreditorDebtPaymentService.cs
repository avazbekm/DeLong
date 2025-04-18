using DeLong.Service.DTOs.CreditorDebtPayments;

namespace DeLong.Service.Interfaces;

public interface ICreditorDebtPaymentService
{
    ValueTask<CreditorDebtPaymentResultDto> AddAsync(CreditorDebtPaymentCreationDto dto);
    ValueTask<CreditorDebtPaymentResultDto> ModifyAsync(CreditorDebtPaymentUpdateDto dto);
    ValueTask<bool> RemoveAsync(long id);
    ValueTask<CreditorDebtPaymentResultDto> RetrieveByIdAsync(long id);
    ValueTask<IEnumerable<CreditorDebtPaymentResultDto>> RetrieveAllAsync();
}