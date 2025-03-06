using DeLong.Service.DTOs.Debts;
using DeLong.Service.DTOs.Payments;

namespace DeLong.Service.DTOs.Sale;

public class SaleCreationDto
{
    public long? CustomerId { get; set; }
    public long? UserId { get; set; }
    public decimal TotalAmount { get; set; }
    public List<PaymentCreationDto> Payments { get; set; } = new();
    public List<DebtCreationDto> Debts { get; set; } = new();
}