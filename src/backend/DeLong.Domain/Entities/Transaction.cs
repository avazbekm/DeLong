using DeLong.Domain.Common;
using DeLong.Domain.Enums;
using System;

namespace DeLong.Domain.Entities;

public class Transaction : Auditable
{
    public long? SupplierIdFrom { get; set; }
    public long BranchId { get; set; }
    public long? BranchIdTo { get; set; }
    public Branch? BranchTo { get; set; }
    public TransactionType TransactionType { get; set; }
    public string Comment { get; set; } = string.Empty;
    public Guid? RequestId { get; set; } // Idempotentlik uchun qo‘shildi
    public List<TransactionItem> Items { get; set; } = new List<TransactionItem>();
}