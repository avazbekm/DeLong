using AutoMapper;
using DeLong.Domain.Entities;
using DeLong.Service.Interfaces;
using DeLong.Application.Interfaces;
using DeLong.Application.Exceptions;
using Microsoft.EntityFrameworkCore;
using DeLong.Service.DTOs.KursDollar;

namespace DeLong.Service.Services;

public class KursDollarService : IKursDollarService
{
    private readonly IMapper mapper;
    private readonly IRepository<KursDollar> kursDollarRepository;
    public KursDollarService(IMapper mapper, IRepository<KursDollar> kursDollarRepository)
    {
        this.mapper = mapper;
        this.kursDollarRepository = kursDollarRepository;
    }
    public async ValueTask<KursDollarResultDto> AddAsync(KursDollarCreationDto dto)
    {
        dto.TodayDate = DateTime.Now.ToString("dd.MM.yyyy");
        var mappedKursDollar = this.mapper.Map<KursDollar>(dto);
        await this.kursDollarRepository.CreateAsync(mappedKursDollar);
        await this.kursDollarRepository.SaveChanges();

        var result = this.mapper.Map<KursDollarResultDto>(mappedKursDollar);
        return result;
    }

    public ValueTask<bool> RemoveAsync(long id)
    {
        throw new NotImplementedException();
    }

    public async ValueTask<KursDollarResultDto> RetrieveByIdAsync()
    {

        var price = await this.kursDollarRepository.GetAll()
                        .OrderByDescending(a => a.CreatedAt)
                        .FirstOrDefaultAsync();

        if (price == null)
            throw new NotFoundException($"Dollar kurs kiritilmagan.");

        var result = this.mapper.Map<KursDollarResultDto>(price);
        return result;
    }

    public async ValueTask<IEnumerable<KursDollarResultDto>> RetrieveAllAsync()
    {
        var prices = await this.kursDollarRepository.GetAll()
           .ToListAsync();
        var result = this.mapper.Map<IEnumerable<KursDollarResultDto>>(prices);
        return result;
    }

}
