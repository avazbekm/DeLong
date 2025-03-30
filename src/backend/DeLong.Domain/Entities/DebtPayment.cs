using DeLong.Domain.Common;

namespace DeLong.Domain.Entities;

public class DebtPayment : Auditable
{
    public long DebtId { get; set; }
    public Debt? Debt { get; set; }

    public decimal Amount { get; set; } // Qancha to‘ladi
    public DateTimeOffset PaymentDate { get; set; }
    public string PaymentMethod { get; set; } = string.Empty; // Yangi: "Cash", "Card", "Dollar"
    public long BranchId { get; set; }
}
