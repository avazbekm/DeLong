using DeLong.Domain.Common;

namespace DeLong.Domain.Entities;

public class CreditorDebtPayment : Auditable
{
    public long CreditorDebtId { get; set; } // Qarzdorlik bilan bog'lanish

    public virtual CreditorDebt? CreditorDebt { get; set; } // Navigation property

    public decimal Amount { get; set; } // To'lov summasi

    public DateTimeOffset PaymentDate { get; set; } // To'lov sanasi

    public string? Description { get; set; } // To'lov uchun izoh (ixtiyoriy)
    public long BranchId { get; set; }
}