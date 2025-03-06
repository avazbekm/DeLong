using DeLong.Service.DTOs.Sale;

namespace DeLong.Service.Interfaces;

public interface ISaleService
{
    ValueTask<SaleResultDto> AddAsync(SaleCreationDto dto);
    ValueTask<SaleResultDto> RetrieveByIdAsync(long id);
    ValueTask<IEnumerable<SaleResultDto>> RetrieveAllAsync();
}