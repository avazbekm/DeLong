using DeLong.Application.DTOs.Invoices;
using DeLong.Service.DTOs.KursDollar;

namespace DeLong.Service.Interfaces;

public interface IKursDollarService
{
    ValueTask<KursDollarResultDto> AddAsync(KursDollarCreationDto dto);
    ValueTask<bool> RemoveAsync(long id);
    ValueTask<KursDollarResultDto> RetrieveByIdAsync();
    ValueTask<IEnumerable<KursDollarResultDto>> RetrieveAllAsync();
}