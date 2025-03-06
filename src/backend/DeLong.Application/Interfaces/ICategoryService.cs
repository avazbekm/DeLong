using DeLong.Domain.Configurations;
using DeLong.Application.DTOs.Categories;

namespace DeLong.Service.Interfaces;

#pragma warning disable
public interface ICategoryService
{
    ValueTask<CategoryResultDto> AddAsync(CategoryCreationDto dto);
    ValueTask<CategoryResultDto> ModifyAsync(CategoryUpdateDto dto);
    ValueTask<bool> RemoveAsync(long id);
    ValueTask<CategoryResultDto> RetrieveByIdAsync(long id);
    ValueTask<IEnumerable<CategoryResultDto>> RetrieveAllAsync(PaginationParams @params, Filter filter, string search = null);
    ValueTask<IEnumerable<CategoryResultDto>> RetrieveAllAsync();
}