namespace DeLong.Domain.Common;

public class Auditable
{
    public long Id { get; set; }

    public long CreatedBy { get; set; }  // Yaratuvchi foydalanuvchi ID
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public long? UpdatedBy { get; set; }  // Yangilagan foydalanuvchi ID
    public DateTimeOffset? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }
}
