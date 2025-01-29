using DeLong.Domain.Enums;
using DeLong.Domain.Common;

namespace DeLong.Domain.Entities;

public class Invoice : Auditable
{
    public long CustomerId { get; set; }
    public required Customer Customer { get; set; }

    public decimal TotalAmount { get; set; }
    public Status Status { get; set; } // Paid, Pending, Cancelled
}