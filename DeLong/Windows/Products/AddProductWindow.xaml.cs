using System.Windows;
using DeLong.DbContexts;
using DeLong.Entities.Products;
using Microsoft.EntityFrameworkCore;

namespace DeLong.Windows.Products
{
    public partial class AddProductWindow : Window
    {
        private readonly AppdbContext _dbContext; 
        public Product NewProduct { get; private set; } 

        public AddProductWindow(AppdbContext dbContext)
        {
            InitializeComponent();
            _dbContext = dbContext; 
        }

        private async void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
            string belgi = txtBelgi.Text.Trim();
            string soni = txtSoni.Text.Trim();
            string narxisumda = txtNarxisumda.Text.Trim();
            string narxidollorda = txtNarxiDollorda.Text.Trim();
            string jaminarxisumda = txtJamiNarxiSumda.Text.Trim();
            string jaminarxidollorda = txtJaminarxiDollorda .Text.Trim();
            
            if (string.IsNullOrWhiteSpace(belgi) ||
                string.IsNullOrWhiteSpace(soni) ||
                string.IsNullOrWhiteSpace(narxisumda) ||
                string.IsNullOrWhiteSpace(narxidollorda) ||
                string.IsNullOrWhiteSpace(jaminarxisumda) ||
                string.IsNullOrWhiteSpace(jaminarxidollorda))
            {
                MessageBox.Show("Iltimos, barcha maydonlarni to'ldiring.", "Xato", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // INN, Xisob Raqam va JSHSHIR qiymatlarini raqamga aylantirish
            if (!int.TryParse(soni, out int inn))
            {
                MessageBox.Show("faqat raqam bo'lishi kerak.", "Xato", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!int.TryParse(narxisumda, out int xisobRaqam))
            {
                MessageBox.Show("faqat raqam bo'lishi kerak.", "Xato", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!int.TryParse(narxidollorda, out int jshshir))
            {
                MessageBox.Show("faqat raqam bo'lishi kerak.", "Xato", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Yangi foydalanuvchini yaratish
            NewProduct = new Product
            {
                Belgi = belgi,
                Soni = int.Parse(soni),
                NarxiSumda = Decimal.Parse(narxisumda),
                NarxiDollorda = Decimal.Parse(narxidollorda),
                JamiNarxiSumda = Decimal.Parse(jaminarxisumda),
                JamiNarxiDollarda = Decimal.Parse(jaminarxidollorda),
            };

            try
            {
                // Yangi foydalanuvchini ma'lumotlar bazasiga qo'shish
                _dbContext.Products.Add(NewProduct);
                await _dbContext.SaveChangesAsync(); // O'zgarishlarni asinxron saqlash

                // Foydalanuvchini muvaffaqiyatli qo'shilgani haqida xabar ko'rsatish
                MessageBox.Show("Foydalanuvchi muvaffaqiyatli qo'shildi.", "Muvaffaqiyat", MessageBoxButton.OK, MessageBoxImage.Information);

                // Oynani yopish
                this.DialogResult = true;
                this.Close();
            }
            catch (DbUpdateException dbEx)
            {
                // Ma'lumotlar bazasi bilan bog'liq xatoliklar uchun maxsus xabar
                MessageBox.Show($"Ma'lumotlar bazasi xatoligi: {dbEx.Message}", "Xato", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                // Xato xabarini ko'rsatish
                MessageBox.Show($"Xatolik yuz berdi: {ex.Message}", "Xato", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
