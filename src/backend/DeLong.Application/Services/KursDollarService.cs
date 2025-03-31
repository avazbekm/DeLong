using AutoMapper;
using DeLong.Application.Exceptions;
using DeLong.Application.Interfaces;
using DeLong.Domain.Entities;
using DeLong.Service.DTOs.KursDollar;
using DeLong.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace DeLong.Service.Services;

public class KursDollarService : AuditableService, IKursDollarService
{
    private readonly IMapper _mapper;
    private readonly IRepository<KursDollar> _kursDollarRepository;

    public KursDollarService(IMapper mapper, IRepository<KursDollar> kursDollarRepository, IHttpContextAccessor httpContextAccessor)
        : base(httpContextAccessor)
    {
        _mapper = mapper;
        _kursDollarRepository = kursDollarRepository;
    }

    public async ValueTask<KursDollarResultDto> AddAsync(KursDollarCreationDto dto)
    {
        dto.TodayDate = DateTime.Now.ToString("dd.MM.yyyy");
        var mappedKursDollar = _mapper.Map<KursDollar>(dto);
        SetCreatedFields(mappedKursDollar); // Auditable maydonlarni qo‘shish
        mappedKursDollar.BranchId = GetCurrentBranchId();
        await _kursDollarRepository.CreateAsync(mappedKursDollar);
        await _kursDollarRepository.SaveChanges();

        return _mapper.Map<KursDollarResultDto>(mappedKursDollar);
    }

    public async ValueTask<bool> RemoveAsync(long id)
    {
        var existKursDollar = await _kursDollarRepository.GetAsync(k => k.Id == id && !k.IsDeleted)
            ?? throw new NotFoundException($"This KursDollar is not found with ID = {id}");

        existKursDollar.IsDeleted = true; // Soft delete
        SetUpdatedFields(existKursDollar); // Auditable maydonlarni yangilash

        _kursDollarRepository.Update(existKursDollar);
        await _kursDollarRepository.SaveChanges();
        return true;
    }

    public async ValueTask<KursDollarResultDto> RetrieveByIdAsync()
    {
        var branchId = GetCurrentBranchId();
        var price = await _kursDollarRepository.GetAll(k => !k.IsDeleted && k.BranchId.Equals(branchId))
                        .OrderByDescending(a => a.CreatedAt)
                        .FirstOrDefaultAsync()
            ?? throw new NotFoundException($"Dollar kurs kiritilmagan.");

        return _mapper.Map<KursDollarResultDto>(price);
    }

    public async ValueTask<IEnumerable<KursDollarResultDto>> RetrieveAllAsync()
    {
        var branchId = GetCurrentBranchId();
        var prices = await _kursDollarRepository.GetAll(k => !k.IsDeleted && k.BranchId.Equals(branchId))
            .ToListAsync();
        return _mapper.Map<IEnumerable<KursDollarResultDto>>(prices);
    }
}