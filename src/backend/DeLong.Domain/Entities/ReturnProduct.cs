using DeLong.Domain.Common;

namespace DeLong.Domain.Entities;

public class ReturnProduct : Auditable
{
    public long? UserId { get; set; }     //jismoniy shaxsdan mahsulot qaytyapti
    public User? User { get; set; }

    public long? CustomerId { get; set; }   // mahsulot yurdik shaxsdan qaytyapti
    public Customer? Customer { get; set; }

    public long SaleId { get; set; }      // savdo qilgan mahsulotlar ro'yxati olish id orqali
    public long ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public string UnitOfMeasure { get; set; } = string.Empty;
    public decimal ReturnSumma { get; set; }
    public string Reason { get; set; } = string.Empty; // Qaytish sababi
}
