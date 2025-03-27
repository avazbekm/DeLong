using AutoMapper;
using DeLong.Application.Interfaces;
using DeLong.Domain.Entities;
using DeLong.Service.DTOs.Payments;
using DeLong.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace DeLong.Service.Services;

public class PaymentService : AuditableService, IPaymentService
{
    private readonly IMapper _mapper;
    private readonly IRepository<Payment> _paymentRepository;

    public PaymentService(IRepository<Payment> paymentRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : base(httpContextAccessor)
    {
        _mapper = mapper;
        _paymentRepository = paymentRepository;
    }

    public async ValueTask<PaymentResultDto> AddAsync(PaymentCreationDto dto)
    {
        var newPayment = _mapper.Map<Payment>(dto);
        SetCreatedFields(newPayment); // Auditable maydonlarni qo‘shish

        await _paymentRepository.CreateAsync(newPayment);
        await _paymentRepository.SaveChanges();
        return _mapper.Map<PaymentResultDto>(newPayment);
    }

    public async ValueTask<IEnumerable<PaymentResultDto>> RetrieveBySaleIdAsync(long saleId)
    {
        var payments = await _paymentRepository.GetAll(p => p.SaleId == saleId && !p.IsDeleted)
            .ToListAsync();
        return _mapper.Map<IEnumerable<PaymentResultDto>>(payments);
    }

    public async ValueTask<IEnumerable<PaymentResultDto>> RetrieveAllAsync()
    {
        var payments = await _paymentRepository.GetAll(p => !p.IsDeleted)
            .ToListAsync();
        return _mapper.Map<IEnumerable<PaymentResultDto>>(payments);
    }
}