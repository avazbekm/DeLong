using DeLong.Domain.Common;
using DeLong.Domain.Enums;

namespace DeLong.Domain.Entities;

public class Transaction : Auditable  // kirim, chiqim, qaytarish amaliyotlari
{
    public long WarehouseIdTo { get; set; }
    public Warehouse? WarehouseTo { get; set; }
    public List<TransactionItem> Items { get; set; } = new List<TransactionItem>();
    public TransactionType TransactionType { get; set; }
    public string Comment { get; set; } = string.Empty;
}