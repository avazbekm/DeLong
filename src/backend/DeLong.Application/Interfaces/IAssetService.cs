using DeLong.Application.DTOs.Assets;
using DeLong.Domain.Configurations;

namespace DeLong.Service.Interfaces;

public interface IAssetService
{
    ValueTask<AssetResultDto> AddAsync(AssetCreationDto dto);
    ValueTask<AssetResultDto> ModifyAsync(AssetUpdateDto dto);
    ValueTask<bool> RemoveAsync(long id);
    ValueTask<AssetResultDto> RetrieveByIdAsync(long id);
    ValueTask<IEnumerable<AssetResultDto>> RetrieveAllAsync(PaginationParams @params, Filter filter, string search = null);
    ValueTask<IEnumerable<AssetResultDto>> RetrieveAllAsync();
}
