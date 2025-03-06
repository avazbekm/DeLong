    namespace DeLong.Service.DTOs.Employee;

    public class EmployeeResultDto
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long WarehouseId { get; set; } // hodim qaysi ombor bilan ishlashini aniqlash uchun
        public string Username { get; set; } = string.Empty;  // Foydalanuvchi nomi (Login)
    }
