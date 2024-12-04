using DeLong.Domain.Common;

namespace DeLong.Domain.Entities;

public class YurdikCustomer : Auditable
{
    public string Name { get; set; } = string.Empty;
    public string Phone {  get; set; } = string.Empty;
    public string TelegramPhone {  get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public int INN { get; set; }
    public string OKONX { get; set; } = string.Empty;
}
public class Invoice
{
    public long? YurdikCustomerId { get; set; }
    public YurdikCustomer? YurdikCustomer { get; set; }

    public long? JismoniyCustomerId { get; set; }
    public JismoniyCustomer? JismoniyCustomer { get; set; }

    public decimal TotalAmount { get; set; }
    public string Status { get; set; } // Paid, Pending, Cancelled
    public DateTime InvoiceDate { get; set; }
}
