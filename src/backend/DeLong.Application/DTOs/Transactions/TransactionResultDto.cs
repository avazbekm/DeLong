using DeLong.Domain.Enums;
using DeLong.Service.DTOs.TransactionItems;

namespace DeLong.Application.DTOs.Transactions;

public class TransactionResultDto
{
    public long Id { get; set; } // Transaksiya identifikatori
    public long WarehouseIdTo { get; set; }   // Kirim ombori
    public List<TransactionItemResultDto> Items { get; set; } = new List<TransactionItemResultDto>(); // Mahsulotlar ro‘yxati
    public TransactionType TransactionType { get; set; }
    public string Comment { get; set; } = string.Empty;
    public DateTime TransactionDate { get; set; } // Yaratilgan sana
    public DateTime? UpdatedDate { get; set; }   // Oxirgi yangilangan sana
    public string CreatedBy { get; set; } = string.Empty; // Kim yaratgan
    public string? LastModifiedBy { get; set; } // Kim oxirgi marta o‘zgartirgan
}
