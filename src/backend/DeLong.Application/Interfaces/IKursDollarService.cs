using DeLong.Application.DTOs.Invoices;
using DeLong.Service.DTOs.KursDollar;

namespace DeLong.Service.Interfaces;

public interface IKursDollarService
{
    ValueTask<KursDollarResultDto> AddAsync(KursDollarCreationDto dto);
    ValueTask<KursDollarResultDto> ModifyAsync(KursDollarUpdateDto dto);
    ValueTask<bool> RemoveAsync(long id);
    ValueTask<KursDollarResultDto> RetrieveByIdAsync(long id);
    ValueTask<IEnumerable<KursDollarResultDto>> RetrieveAllAsync();
}