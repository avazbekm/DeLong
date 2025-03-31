using DeLong.Domain.Common;

namespace DeLong.Domain.Entities;

public class Employee : Auditable
{
    public long UserId { get; set; }
    public long BranchId { get; set; } // hodim qaysi filial bilan ishlashini aniqlash uchun
    public string Username { get; set; } = string.Empty;  // Foydalanuvchi nomi (Login)
    public string Password { get; set; } = string.Empty; // Parol (hashlangan)

    public virtual User User { get; set; }
    public virtual Branch Branch { get; set; }
}