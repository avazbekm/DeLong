using DeLong.Service.DTOs.Debts;
using DeLong.Service.DTOs.Payments;

namespace DeLong.Service.DTOs.Sale;

public class SaleResultDto
{
    public long Id { get; set; }
    public long? CustomerId { get; set; }
    public string? CustomerName { get; set; } = string.Empty;  // Mijoz ismi
    public long? UserId { get; set; }
    public string? UserName { get; set; } = string.Empty;  // Mijoz ismi
    public decimal TotalAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal RemainingAmount { get; set; }
    public string Status { get; set; } = string.Empty; // Enum string sifatida
    public long CreatedBy { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    public List<PaymentResultDto> Payments { get; set; } = new();
    public List<DebtResultDto> Debts { get; set; } = new();
}