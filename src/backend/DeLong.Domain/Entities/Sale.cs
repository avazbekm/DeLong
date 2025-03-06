using DeLong.Domain.Enums;
using DeLong.Domain.Common;

namespace DeLong.Domain.Entities;

public class Sale:Auditable
{
    public long? CustomerId { get; set; }
    public Customer? Customer { get; set; }

    public long? UserId { get; set; }
    public User? User { get; set; }

    public decimal TotalAmount { get; set; }
    public decimal PaidAmount => Payments.Sum(p => p.Amount);
    public decimal RemainingAmount => TotalAmount - PaidAmount;

    public SaleStatus Status =>
        (PaidAmount == 0) ? SaleStatus.Credit :
        (RemainingAmount > 0) ? SaleStatus.Partial : SaleStatus.Paid;

    public List<Payment> Payments { get; set; } = new();
    public List<Debt> Debts { get; set; } = new();
}
