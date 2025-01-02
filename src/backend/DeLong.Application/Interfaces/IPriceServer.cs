using DeLong.Service.DTOs.Prices;
using DeLong.Domain.Configurations;

namespace DeLong.Service.Interfaces;

public interface IPriceServer
{
    ValueTask<PriceResultDto> AddAsync(PriceCreationDto dto);
    ValueTask<PriceResultDto> ModifyAsync(PriceUpdateDto dto);
    ValueTask<bool> RemoveAsync(long id);
    ValueTask<PriceResultDto> RetrieveByIdAsync(long id);
    ValueTask<IEnumerable<PriceResultDto>> RetrieveAllAsync(PaginationParams @params, Filter filter, string search = null);
    ValueTask<IEnumerable<PriceResultDto>> RetrieveAllAsync();
}
