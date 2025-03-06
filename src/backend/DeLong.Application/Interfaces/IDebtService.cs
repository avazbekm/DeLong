using DeLong.Service.DTOs.Debts;

namespace DeLong.Service.Interfaces;

public interface IDebtService
{
    ValueTask<DebtResultDto> AddAsync(DebtCreationDto dto);
    ValueTask<DebtResultDto> ModifyAsync(DebtUpdateDto dto);
    ValueTask<bool> RemoveAsync(long id);
    ValueTask<DebtResultDto> RetrieveByIdAsync(long id);
    ValueTask<IEnumerable<DebtResultDto>> RetrieveAllAsync();
    ValueTask<IEnumerable<DebtResultDto>> RetrieveBySaleIdAsync(long saleId); // Yangi metod qo‘shildi
    ValueTask<Dictionary<string, List<DebtResultDto>>> RetrieveAllGroupedByCustomerAsync(); // Yangi metod
}