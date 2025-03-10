using DeLong.Domain.Enums;
using DeLong.Service.DTOs.TransactionItems;

namespace DeLong.Application.DTOs.Transactions;

public class TransactionCreationDto
{
    public long WarehouseIdTo { get; set; }   // Kirim ombori
    public List<TransactionItemCreationDto> Items { get; set; } = new List<TransactionItemCreationDto>(); // Mahsulotlar ro‘yxati
    public TransactionType TransactionType { get; set; }
    public string Comment { get; set; } = string.Empty;
}
