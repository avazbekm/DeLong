namespace DeLong.Application.DTOs.Customers;

public class CustomerUpdateDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int INN { get; set; }
    public string MFO { get; set; } = string.Empty;
    public long BankAccount { get; set; }
    public string BankName { get; set; } = string.Empty;
    public string OKONX { get; set; } = string.Empty;
    public string YurAddress { get; set; } = string.Empty;

    // firma rahbari malumotlari
    public long UserId { get; set; }
    public UserResultDto User { get; set; }
}
