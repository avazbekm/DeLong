using System;

namespace DeLong.Domain.Common;

public class Auditable
{
    public long Id { get; set; }

    public long CreatedBy { get; set; }  // Yaratuvchi foydalanuvchi ID
    public DateTimeOffset CreatedAt { get; set; }  // Default qiymat konstruktor yoki serviceda o‘rnatiladi

    public long? UpdatedBy { get; set; }  // Yangilagan foydalanuvchi ID
    public DateTimeOffset? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    // Konstruktor
    public Auditable()
    {
        CreatedAt = GetUzbekistanTime();
    }

    // O‘zbekiston vaqtini olish uchun metod
    private static DateTimeOffset GetUzbekistanTime()
    {
        // O‘zbekiston vaqti uchun TimeZoneInfo
        TimeZoneInfo uzbTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
        return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, uzbTimeZone);
    }
}