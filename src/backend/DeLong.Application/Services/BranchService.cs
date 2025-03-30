using AutoMapper;
using DeLong.Domain.Entities;
using DeLong.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using DeLong.Service.DTOs.Branchs;
using DeLong.Application.Exceptions;
using DeLong.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DeLong.Service.Services;

public class BranchService : AuditableService, IBranchService
{
    private readonly IRepository<Branch> _repository;
    private readonly IMapper _mapper;

    public BranchService(IRepository<Branch> repository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : base(httpContextAccessor) // AuditableService konstruktorini chaqirish
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async ValueTask<BranchResultDto> AddAsync(BranchCreationDto dto)
    {
        var branch = _mapper.Map<Branch>(dto);
        SetCreatedFields(branch); // Yaratilish vaqti va foydalanuvchi ID qo‘shiladi
        branch.CreatedBy = GetCurrentBranchId(); // Joriy filial ID’si qo‘shiladi (agar logika mos bo‘lsa)

        await _repository.CreateAsync(branch);
        await _repository.SaveChanges();

        return _mapper.Map<BranchResultDto>(branch);
    }

    public async ValueTask<BranchResultDto> ModifyAsync(BranchUpdateDto dto)
    {
        var existBranch = await _repository.GetAsync(b => b.Id == dto.Id && !b.IsDeleted)
            ?? throw new NotFoundException($"Branch not found with ID = {dto.Id}");

        _mapper.Map(dto, existBranch);
        SetUpdatedFields(existBranch); // Yangilanish vaqti va foydalanuvchi ID qo‘shiladi

        _repository.Update(existBranch);
        await _repository.SaveChanges();

        return _mapper.Map<BranchResultDto>(existBranch);
    }

    public async ValueTask<bool> RemoveAsync(long id)
    {
        var existBranch = await _repository.GetAsync(b => b.Id == id && !b.IsDeleted)
            ?? throw new NotFoundException($"Branch not found with ID = {id}");

        existBranch.IsDeleted = true; // Soft delete
        SetUpdatedFields(existBranch); // Yangilanish vaqti va foydalanuvchi ID qo‘shiladi

        _repository.Update(existBranch); // Delete o‘rniga Update
        await _repository.SaveChanges();
        return true;
    }

    public async ValueTask<BranchResultDto> RetrieveByIdAsync(long id)
    {
        var existBranch = await _repository.GetAsync(b => b.Id == id && !b.IsDeleted)
            ?? throw new NotFoundException($"Branch not found with ID = {id}");

        return _mapper.Map<BranchResultDto>(existBranch);
    }

    public async ValueTask<IEnumerable<BranchResultDto>> RetrieveAllAsync()
    {
        var branches = await _repository.GetAll(b => !b.IsDeleted)
            .ToListAsync();

        return _mapper.Map<IEnumerable<BranchResultDto>>(branches);
    }
}