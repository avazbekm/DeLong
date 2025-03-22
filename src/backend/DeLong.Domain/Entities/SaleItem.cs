using DeLong.Domain.Common;

namespace DeLong.Domain.Entities;

public class SaleItem : Auditable
{
    public long SaleId { get; set; }
    public required Sale Sale { get; set; }

    public long ProductId { get; set; }
    public required Product Product { get; set; }
    public string UnitOfMeasure { get; set; } = string.Empty;

    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
