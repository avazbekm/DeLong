using AutoMapper;
using DeLong.Application.Exceptions;
using DeLong.Application.Interfaces;
using DeLong.Domain.Entities;
using DeLong.Service.DTOs.KursDollar;
using DeLong.Service.DTOs.Prices;
using DeLong.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

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

        var mappedKursDollar = this.mapper.Map<KursDollar>(dto);
        await this.kursDollarRepository.CreateAsync(mappedKursDollar);
        await this.kursDollarRepository.SaveChanges();

        var result = this.mapper.Map<KursDollarResultDto>(mappedKursDollar);
        return result;
    }

    public ValueTask<KursDollarResultDto> ModifyAsync(KursDollarUpdateDto dto)
    {
        throw new NotImplementedException();
    }

    public ValueTask<bool> RemoveAsync(long id)
    {
        throw new NotImplementedException();
    }

    public async ValueTask<KursDollarResultDto> RetrieveByIdAsync(long id)
    {
        KursDollar existKursDollar = await this.kursDollarRepository.GetAsync(u => u.Id.Equals(id))
       ?? throw new NotFoundException($"This Price is not found with ID = {id}");

        var result = this.mapper.Map<KursDollarResultDto>(existKursDollar);
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
