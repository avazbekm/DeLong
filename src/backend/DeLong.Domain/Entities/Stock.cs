using DeLong.Domain.Common;

namespace DeLong.Domain.Entities;

public class Stock : Auditable // ombordagi qoldiq
{
    public long WarehouseId { get; set; }
    public required Warehouse Warehouse { get; set; }

    public long ProductId { get; set; }
    public Product? Product { get; set; }

    public decimal Quantity { get; set; } // maxsulot soni
    public decimal MinStockLevel { get; set; }  // minimal qoldiqni belgilab qo'yish
}