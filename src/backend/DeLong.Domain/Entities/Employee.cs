using DeLong.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace DeLong.Domain.Entities;

public class Employee : Auditable
{
    public long UserId { get; set; }
    public long WarehouseId { get; set; } // hodim qaysi ombor bilan ishlashini aniqlash uchun
    public string Username { get; set; } = string.Empty;  // Foydalanuvchi nomi (Login)
    public string Password { get; set; } = string.Empty; // Parol (hashlangan)
}