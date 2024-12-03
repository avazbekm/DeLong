namespace DeLong.Entities.Products;

public class Product:Auditable
{
    public string Belgi { get; set; }
    public int Soni { get; set; }
    public decimal NarxiSumda { get; set; }
    public decimal JamiNarxiSumda { get; set; }
    public decimal NarxiDollorda { get; set; }
    public decimal JamiNarxiDollarda { get; set; }
}
