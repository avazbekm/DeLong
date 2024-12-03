using System.Windows;
using DeLong.DbContexts;
using DeLong.Entities.Products;
using Microsoft.EntityFrameworkCore;

namespace DeLong.Windows.Products
{
    public partial class ProductEditWindow : Window
    {
        private readonly AppdbContext _dbContext; // AppDbContext obyektini qo'shish
        public Product UpdatedProduct { get; private set; }
        private readonly Product _originalProduct; // Asl product obyektini saqlash

        public ProductEditWindow(AppdbContext dbContext, Product product)
        {
            InitializeComponent();

            _dbContext = dbContext;
            _originalProduct = product;

            // Product ma'lumotlarini formaga yuklash
            txtBelgi.Text = product.Belgi;
            txtNarxisumda.Text = product.NarxiSumda.ToString();
            txtNarxiDollorda.Text = product.NarxiDollorda.ToString();
            txtJamiNarxiSumda.Text = product.JamiNarxiSumda.ToString();
            txtJaminarxiDollorda.Text = product.JamiNarxiDollarda.ToString();
        }

        private void EditProductButton_Click(object sender, RoutedEventArgs e)
        {
            // Narxi va jami narxlarni raqam formatida o'qish
            if (decimal.TryParse(txtNarxisumda.Text, out decimal narxiSumda) &&
                decimal.TryParse(txtNarxiDollorda.Text, out decimal narxiDollorda) &&
                decimal.TryParse(txtJamiNarxiSumda.Text, out decimal jamiNarxiSumda) &&
                decimal.TryParse(txtJaminarxiDollorda.Text, out decimal jamiNarxiDollorda))
            {
                // Productni yangilash
                _originalProduct.Belgi = txtBelgi.Text;
                _originalProduct.NarxiSumda = narxiSumda;
                _originalProduct.NarxiDollorda = narxiDollorda;
                _originalProduct.JamiNarxiSumda = jamiNarxiSumda;
                _originalProduct.JamiNarxiDollarda = jamiNarxiDollorda;

                try
                {
                    // Ma'lumotlar bazasiga o'zgarishlarni saqlash
                    _dbContext.Entry(_originalProduct).State = EntityState.Modified;
                    _dbContext.SaveChanges();

                    MessageBox.Show("Product muvaffaqiyatli yangilandi.", "Muvaffaqiyat", MessageBoxButton.OK, MessageBoxImage.Information);

                    this.DialogResult = true; // Oynani yopishdan oldin natijani ko'rsatish
                    this.Close(); // Oynani yopish
                }
                catch (DbUpdateException ex)
                {
                    MessageBox.Show($"Xatolik yuz berdi: {ex.Message}", "Xato", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Iltimos, to'g'ri qiymatlar kiriting!", "Xato", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
