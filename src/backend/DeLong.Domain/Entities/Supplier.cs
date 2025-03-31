using DeLong.Domain.Common;

namespace DeLong.Domain.Entities;

public class Supplier : Auditable // yetkazib beruvchilar
{
    public string Name { get; set; } = string.Empty;
    public string ContactInfo { get; set; } = string.Empty; // aloqa uchun telefon nomer
    public long BranchId { get; set; }
}

