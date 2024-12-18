using AutoMapper;
using DeLong.Domain.Entities;
using DeLong.Service.Interfaces;
using DeLong.Domain.Configurations;
using DeLong.Application.DTOs.Users;
using DeLong.Application.Exceptions;
using DeLong.Application.Interfaces;
using DeLong.Application.DTOs.Customers;
using DeLong.Application.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DeLong.Service.Services;

public class UserService : IUserService
{
    private readonly IMapper mapper;
    private readonly IRepository<User> userRepository;

    public UserService(IRepository<User> userRepository, IMapper mapper)
    {
        this.mapper = mapper;
        this.userRepository = userRepository;
    }

    public async ValueTask<UserResultDto> AddAsync(UserCreationDto dto)
    {
        var existUser = await this.userRepository.GetAsync(u => u.JSHSHIR.Equals(dto.JSHSHIR));
        if (existUser is not null)
            throw new AlreadyExistException($"This customer is already exists with JSHSHIR = {dto.JSHSHIR}");

        var mappedUser = this.mapper.Map<User>(dto);
        await this.userRepository.CreateAsync(mappedUser);
        await this.userRepository.SaveChanges();

        var result = this.mapper.Map<UserResultDto>(mappedUser);
        return result;
    }

    public async ValueTask<UserResultDto> ModifyAsync(UserUpdateDto dto)
    {
        User existUser = await this.userRepository.GetAsync(u => u.Id.Equals(dto.Id))
            ?? throw new NotFoundException($"This User is not found with ID = {dto.Id}");

        var mappedUser = this.mapper.Map(dto, existUser);
        this.userRepository.Update(mappedUser);
        await this.userRepository.SaveChanges();

        var result = this.mapper.Map<UserResultDto>(mappedUser);
        return result;
    }

    public async ValueTask<bool> RemoveAsync(long id)
    {
        User existUser = await this.userRepository.GetAsync(u => u.Id.Equals(id))
            ?? throw new NotFoundException($"This customer is not found with ID = {id}");

        this.userRepository.Delete(existUser);
        await this.userRepository.SaveChanges();
        return true;
    }

    public async ValueTask<IEnumerable<UserResultDto>> RetrieveAllAsync(PaginationParams @params, Filter filter, string search = null)
    {
        var users = await this.userRepository.GetAll()
          .ToPaginate(@params)
          .OrderBy(filter)
          .ToListAsync();

        var result = users.Where(user => user.LastName.Contains(search, StringComparison.OrdinalIgnoreCase));
        var mappedUsers = this.mapper.Map<List<UserResultDto>>(result);
        return mappedUsers;
    }

    public async ValueTask<IEnumerable<UserResultDto>> RetrieveAllAsync()
    {
        var users = await this.userRepository.GetAll()
          .ToListAsync();
        var result = this.mapper.Map<IEnumerable<UserResultDto>>(users);
        return result;
    }

    public async ValueTask<UserResultDto> RetrieveByIdAsync(long id)
    {
        User existUser = await this.userRepository.GetAsync(u => u.Id.Equals(id))
             ?? throw new NotFoundException($"This customer is not found with ID = {id}");

        var result = this.mapper.Map<UserResultDto>(existUser);
        return result;
    }

    public async ValueTask<UserResultDto> RetrieveByJSHSHIRAsync(long Jshshir)
    {
        User existUser = await this.userRepository.GetAsync(user => user.JSHSHIR.Equals(Jshshir))
            ?? throw new NotFoundException($"This customer is not found with JSHSHIR = {Jshshir}");

        var result = this.mapper.Map<UserResultDto>(existUser);
        return result;
    }
}
