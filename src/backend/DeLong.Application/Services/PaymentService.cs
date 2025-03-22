using AutoMapper;
using DeLong.Application.Interfaces;
using DeLong.Domain.Entities;
using DeLong.Service.DTOs.Payments;
using DeLong.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DeLong.Service.Services;

public class PaymentService : IPaymentService
{
    private readonly IMapper mapper;
    private readonly IRepository<Payment> paymentRepository;

    public PaymentService(IRepository<Payment> paymentRepository, IMapper mapper)
    {
        this.mapper = mapper;
        this.paymentRepository = paymentRepository;
    }

    public async ValueTask<PaymentResultDto> AddAsync(PaymentCreationDto dto)
    {
        var newPayment = this.mapper.Map<Payment>(dto);
        await this.paymentRepository.CreateAsync(newPayment);
        await this.paymentRepository.SaveChanges();
        return this.mapper.Map<PaymentResultDto>(newPayment);
    }

    public async ValueTask<IEnumerable<PaymentResultDto>> RetrieveBySaleIdAsync(long saleId)
    {
        var payments = await this.paymentRepository.GetAll(p => p.SaleId == saleId).ToListAsync();
        return this.mapper.Map<IEnumerable<PaymentResultDto>>(payments);
    }

    public async ValueTask<IEnumerable<PaymentResultDto>> RetrieveAllAsync()
    {
        var payments = await this.paymentRepository.GetAll().ToListAsync();
        return this.mapper.Map<IEnumerable<PaymentResultDto>>(payments);
    }
}
