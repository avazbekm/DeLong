using DeLong.Application.DTOs.Users;
using DeLong.Domain.Configurations;
using DeLong.Domain.Entities;

namespace DeLong.Service.Interfaces;

public interface IUserService
{
    ValueTask<UserResultDto> AddAsync(UserCreationDto dto);
    ValueTask<UserResultDto> ModifyAsync(UserUpdateDto dto);
    ValueTask<bool> RemoveAsync(long id);
    ValueTask<UserResultDto> RetrieveByIdAsync(long id);
    ValueTask<UserResultDto> RetrieveByJSHSHIRAsync(string Jshshir);
    ValueTask<IEnumerable<UserResultDto>> RetrieveAllAsync(PaginationParams @params, Filter filter, string search = null);
    ValueTask<IEnumerable<UserResultDto>> RetrieveAllAsync();
    void CreateSeedUserAsync(User user); // Seed uchun
    ValueTask<UserResultDto> GetLastUser();
    ValueTask<bool> AnyUsersAsync(); // Qo‘shildi
}