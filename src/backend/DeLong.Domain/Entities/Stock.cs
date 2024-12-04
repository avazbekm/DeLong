﻿using DeLong.Domain.Common;

namespace DeLong.Domain.Entities;

public class Stock : Auditable // ombordagi qoldiq
{
    public int WarehouseId { get; set; }
    public required Warehouse Warehouse { get; set; }

    public int ProductId { get; set; }
    public Product? Product { get; set; }

    public decimal Quantity { get; set; } // maxsulot soni
    public decimal MinStockLevel { get; set; }  // kam qoldiqni belgilab qo'yish
}
