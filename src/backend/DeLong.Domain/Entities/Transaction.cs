using DeLong.Domain.Common;
using DeLong.Domain.Enums;

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

    public long? JismoniyCustomerId { get; set; }  // jismoniy mijozlar
    public JismoniyCustomer? JismoniyCustomer { get; set; }

    public long? YurdikCustomerId { get; set; }  // Yurdik mijozlar
    public YurdikCustomer? YurdikCustomer { get; set; }
}
