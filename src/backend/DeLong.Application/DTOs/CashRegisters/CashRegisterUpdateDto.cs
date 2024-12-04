namespace DeLong.Application.DTOs.CashRegisters;

internal class CashRegisterUpdateDto
{
    public long Id { get; set; }
    public int WarehouseId { get; set; }
    public decimal Balance { get; set; }
}
