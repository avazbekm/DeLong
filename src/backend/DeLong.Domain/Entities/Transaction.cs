using DeLong.Domain.Enums;
using DeLong.Domain.Common;

namespace DeLong.Domain.Entities;

public class Transaction : Auditable  // kirim, chiqim, qaytarish amaliyotlari
{
    public long? WarehouseIdFrom { get; set; } // qaysidir ombordan chiqishi
    public Warehouse? WarehouseFrom { get; set; }

    public long? WarehouseIdTo { get; set; }  // qaysidir omborga kelishi
    public Warehouse? WarehouseTo { get; set; }

    public long ProductId { get; set; }
    public required Product Product { get; set; }
    public decimal PriceProduct { get; set; } // maxsulot narxi

    public TransactionType TransactionType { get; set; } // kirim, chiqim, qaytarish 
    public decimal Quantity { get; set; }  // maxsulot soni

    public long? CustomerId { get; set; }  //  mijozlar
    public Customer? Customer { get; set; }

    public long? SupplierId { get; set; } // yetkazib beruvchi
    public Supplier? Supplier { get; set; }
}
