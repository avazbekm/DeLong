using DeLong.Service.DTOs.DebtPayments;

namespace DeLong.Service.Interfaces;

public interface IDebtPaymentService
{
    ValueTask<DebtPaymentResultDto> AddAsync(DebtPaymentCreationDto dto);
    ValueTask<IEnumerable<DebtPaymentResultDto>> RetrieveByDebtIdAsync(long debtId);
    ValueTask<IEnumerable<DebtPaymentResultDto>> RetrieveAllAsync();
}