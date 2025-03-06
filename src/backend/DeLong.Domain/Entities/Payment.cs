using DeLong.Domain.Common;
using DeLong.Domain.Enums;

namespace DeLong.Domain.Entities;

public class Payment : Auditable
{
    public long SaleId { get; set; }
    public Sale? Sale { get; set; }

    public decimal Amount { get; set; }
    public PaymentType Type { get; set; } // Cash, Card, Transfer, Credit, Dollar
}