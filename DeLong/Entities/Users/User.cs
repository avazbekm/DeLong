namespace DeLong.Entities.Users;

public class User:Auditable
{
    public string FIO { get; set; }
    public string Telefon { get; set; }
    public string Adres { get; set; }
    public string TelegramRaqam { get; set; }
    public int INN { get; set; }
    public string OKONX { get; set; }
    public string XisobRaqam { get; set; }
    public string JSHSHIR { get; set; }
    public string Bank { get; set; }
    public string FirmaAdres { get; set; }
}
