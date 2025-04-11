using DeLong.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace DeLong.Domain.Entities;

public class ChangeHistory : Auditable
{
    // Bog'lanadigan jadvalning ID si (masalan, PriceId, SupplierId va hokazo)
    public long EntityId { get; set; }

    // Jadval nomi (masalan, "Price", "Supplier")
    [StringLength(50)]
    public string EntityName { get; set; } = string.Empty;

    // Eski qiymatlar JSON sifatida
    public string? OldValues { get; set; }

    // Yangi qiymatlar JSON sifatida
    public string? NewValues { get; set; }

    // O'zgarish turi: "Insert", "Update", "Delete"
    [StringLength(20)]
    public string ChangeType { get; set; } = string.Empty;
}
