using DeLong.Domain.Enums;

namespace DeLong.Service.DTOs.Employee;

public class EmployeeResultDto
{
    public long Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public Role Role { get; set; }
    public string Username { get; set; } = string.Empty;  // Foydalanuvchi nomi (Login)

    public long BranchId { get; set; } // hodim qaysi filial bilan ishlashini aniqlash uchun
    public long CreatedBy { get; set; }
}
