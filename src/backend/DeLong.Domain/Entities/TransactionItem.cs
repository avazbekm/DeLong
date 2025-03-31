using DeLong.Domain.Common;

namespace DeLong.Domain.Entities;

public class TransactionItem : Auditable
{
    public long TransactionId { get; set; }
    public long ProductId { get; set; }
    public Product? Product { get; set; }
    public decimal Quantity { get; set; }
    public string UnitOfMeasure { get; set; } = string.Empty;
    public decimal PriceProduct { get; set; }
    public long BranchId { get; set; }
}