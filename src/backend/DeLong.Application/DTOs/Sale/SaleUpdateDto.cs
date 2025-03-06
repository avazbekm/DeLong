using DeLong.Service.DTOs.Debts;
using DeLong.Service.DTOs.Payments;

namespace DeLong.Service.DTOs.Sale;

public class SaleUpdateDto
{
    public long Id { get; set; }
    public long? CustomerId { get; set; }
    public long? UserId { get; set; }
    public decimal TotalAmount { get; set; }
    public List<PaymentUpdateDto> Payments { get; set; } = new();
    public List<DebtUpdateDto> Debts { get; set; } = new();
}