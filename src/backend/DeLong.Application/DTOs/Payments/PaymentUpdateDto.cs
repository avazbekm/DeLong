using DeLong.Domain.Enums;

namespace DeLong.Service.DTOs.Payments;

public class PaymentUpdateDto
{
    public long Id { get; set; }  // To‘lovning ID’si
    public long SaleId { get; set; }
    public decimal Amount { get; set; }
    public PaymentType Type { get; set; }
}
