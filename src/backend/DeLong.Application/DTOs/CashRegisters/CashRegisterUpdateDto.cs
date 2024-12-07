namespace DeLong.Application.DTOs.CashRegisters;

public class CashRegisterUpdateDto
{
    public long Id { get; set; }
    public int WarehouseId { get; set; }
    public decimal Balance { get; set; }
}
