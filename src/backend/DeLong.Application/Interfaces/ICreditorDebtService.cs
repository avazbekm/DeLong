using DeLong.Service.DTOs.CreditorDebts;

namespace DeLong.Service.Interfaces;

public interface ICreditorDebtService
{
    ValueTask<CreditorDebtResultDto> AddAsync(CreditorDebtCreationDto dto);
    ValueTask<CreditorDebtResultDto> ModifyAsync(CreditorDebtUpdateDto dto);
    ValueTask<bool> RemoveAsync(long id);
    ValueTask<CreditorDebtResultDto> RetrieveByIdAsync(long id);
    ValueTask<IEnumerable<CreditorDebtResultDto>> RetrieveAllAsync();
}
