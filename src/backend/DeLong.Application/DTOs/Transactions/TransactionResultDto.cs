using DeLong.Domain.Entities;
using DeLong.Domain.Enums;

namespace DeLong.Application.DTOs.Transactions;

public class TransactionResultDto
{
    public long Id { get; set; }
    public Warehouse? WarehouseFrom { get; set; }
    public Warehouse? WarehouseTo { get; set; }
    public required Product Product { get; set; }
    public decimal PriceProduct { get; set; } // maxsulot narxi
    public TransactionType TransactionType { get; set; } // kirim, chiqim, qaytarish 
    public decimal Quantity { get; set; }  // maxsulot soni
    public Customer? Customer { get; set; }
    public Supplier? Supplier { get; set; }
}
