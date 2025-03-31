using DeLong.Domain.Common;

namespace DeLong.Domain.Entities;

public class Price : Auditable
{
    public long? SupplierId { get; set; }
    public Supplier? Supplier { get; set; }

    public long ProductId { get; set; }
    public decimal CostPrice { get; set; }  // Tan narxi
    public decimal SellingPrice { get; set; }  // Sotish narxi
    public string UnitOfMeasure { get; set; } = string.Empty; // kg,dona,karobka,litr
    public decimal Quantity { get; set; } // miqdori
    public long BranchId { get; set; }
}