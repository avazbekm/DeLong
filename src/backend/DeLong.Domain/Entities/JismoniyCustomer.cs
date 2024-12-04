using DeLong.Domain.Common;

namespace DeLong.Domain.Entities;

public class JismoniyCustomer : Auditable
{
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string TelegramPhone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public long JSHSHR { get; set; }
}
