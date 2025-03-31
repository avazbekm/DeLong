using DeLong.Domain.Enums;
using DeLong.Service.DTOs.TransactionItems;

namespace DeLong.Application.DTOs.Transactions;

public class TransactionResultDto
{
    public long Id { get; set; } // Tranzaksiya ID
    public long? SupplierIdFrom { get; set; } // Yetkazib beruvchi ID
    public long BranchId { get; set; } // Manba filial ID
    public long? BranchIdTo { get; set; } // Qabul qiluvchi filial ID
    public TransactionType TransactionType { get; set; } // Tranzaksiya turi
    public string Comment { get; set; } = string.Empty; // Izoh
    public List<TransactionItemResultDto> Items { get; set; } = new List<TransactionItemResultDto>(); // Tranzaksiya elementlari
    public DateTimeOffset CreatedAt { get; set; } // Yaratilgan vaqt (Auditable’dan)
    public DateTimeOffset? UpdatedAt { get; set; } // Yangilangan vaqt (Auditable’dan)
}
