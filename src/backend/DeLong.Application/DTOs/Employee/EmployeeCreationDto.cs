
namespace DeLong.Service.DTOs.Employee;

public class EmployeeCreationDto
{
    public long UserId { get; set; }
    public long BranchId { get; set; } // hodim qaysi filial bilan ishlashini aniqlash uchun
    public string Username { get; set; } = string.Empty;  // Foydalanuvchi nomi (Login)
    public string Password { get; set; } = string.Empty; // Parol (hashlangan)
}