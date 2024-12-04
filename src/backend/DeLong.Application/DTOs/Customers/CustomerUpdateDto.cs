namespace DeLong.Application.DTOs.Customers;

public class CustomerUpdateDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string TelegramPhone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public int? INN { get; set; }
    public string? OKONX { get; set; } = string.Empty;
    public long? JSHSHR { get; set; }
}
