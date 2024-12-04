using DeLong.Domain.Enums;

namespace DeLong.Application.DTOs.Transactions;

public class TransactionCreationDto
{
    public long? WarehouseIdFrom { get; set; } // qaysidir ombordan chiqishi
    public long? WarehouseIdTo { get; set; }  // qaysidir omborga kelishi
    public long ProductId { get; set; }
    public decimal PriceProduct { get; set; } // maxsulot narxi

    public TransactionType TransactionType { get; set; } // kirim, chiqim, qaytarish 
    public decimal Quantity { get; set; }  // maxsulot soni

    public long? CustomerId { get; set; }  //  mijozlar
    public long? SupplierId { get; set; } // yetkazib beruvchi
}
