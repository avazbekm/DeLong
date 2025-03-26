using AutoMapper;
using DeLong.Domain.Entities;
using DeLong.Service.Interfaces;
using DeLong.Domain.Configurations;
using DeLong.Application.DTOs.Users;
using DeLong.Application.Exceptions;
using DeLong.Application.Extensions;
using DeLong.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace DeLong.Service.Services;

public class UserService : AuditableService, IUserService
{
    private readonly IMapper _mapper;
    private readonly IRepository<User> _userRepository;

    public UserService(IRepository<User> userRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : base(httpContextAccessor)
    {
        _mapper = mapper;
        _userRepository = userRepository;
    }

    public async ValueTask<UserResultDto> AddAsync(UserCreationDto dto)
    {
        var existUser = await _userRepository.GetAsync(u => u.JSHSHIR.Equals(dto.JSHSHIR) && !u.IsDeleted);
        if (existUser != null)
            throw new AlreadyExistException($"This user is already exists with JSHSHIR = {dto.JSHSHIR}");

        var mappedUser = _mapper.Map<User>(dto);
        SetCreatedFields(mappedUser); // Auditable maydonlarni qo‘shish

        await _userRepository.CreateAsync(mappedUser);
        await _userRepository.SaveChanges();
        return _mapper.Map<UserResultDto>(mappedUser);
    }

    public async ValueTask<UserResultDto> ModifyAsync(UserUpdateDto dto)
    {
        var existUser = await _userRepository.GetAsync(u => u.Id.Equals(dto.Id) && !u.IsDeleted)
            ?? throw new NotFoundException($"This user is not found with ID = {dto.Id}");

        _mapper.Map(dto, existUser);
        SetUpdatedFields(existUser); // Auditable maydonlarni yangilash

        _userRepository.Update(existUser);
        await _userRepository.SaveChanges();
        return _mapper.Map<UserResultDto>(existUser);
    }

    public async ValueTask<bool> RemoveAsync(long id)
    {
        var existUser = await _userRepository.GetAsync(u => u.Id.Equals(id) && !u.IsDeleted)
            ?? throw new NotFoundException($"This user is not found with ID = {id}");

        existUser.IsDeleted = true; // Soft delete
        SetUpdatedFields(existUser); // Auditable maydonlarni yangilash

        _userRepository.Update(existUser);
        await _userRepository.SaveChanges();
        return true;
    }

    public async ValueTask<UserResultDto> RetrieveByIdAsync(long id)
    {
        var existUser = await _userRepository.GetAsync(u => u.Id.Equals(id) && !u.IsDeleted)
            ?? throw new NotFoundException($"This user is not found with ID = {id}");

        return _mapper.Map<UserResultDto>(existUser);
    }

    public async ValueTask<UserResultDto> RetrieveByJSHSHIRAsync(string jshshir)
    {
        var existUser = await _userRepository.GetAsync(u => u.JSHSHIR.Equals(jshshir) && !u.IsDeleted)
            ?? throw new NotFoundException($"This user is not found with JSHSHIR = {jshshir}");

        return _mapper.Map<UserResultDto>(existUser);
    }

    public async ValueTask<IEnumerable<UserResultDto>> RetrieveAllAsync(PaginationParams @params, Filter filter, string search = null)
    {
        var query = _userRepository.GetAll(u => !u.IsDeleted);

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(u => u.LastName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                                     u.FirstName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                                     u.JSHSHIR.Contains(search, StringComparison.OrdinalIgnoreCase));
        }

        var users = await query
            .ToPaginate(@params)
            .OrderBy(filter)
            .ToListAsync();

        return _mapper.Map<IEnumerable<UserResultDto>>(users);
    }

    public async ValueTask<IEnumerable<UserResultDto>> RetrieveAllAsync()
    {
        var users = await _userRepository.GetAll(u => !u.IsDeleted)
            .ToListAsync();
        return _mapper.Map<IEnumerable<UserResultDto>>(users);
    }
}