using System.Windows;
using DeLong.DbContexts;
using DeLong.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace DeLong.Windows.Users
{
    public partial class AddUserWindow : Window
    {
        private readonly AppdbContext _dbContext; // AppDbContext uchun private xususiyat
        public User NewUser { get; private set; } // Yangi foydalanuvchi

        public AddUserWindow(AppdbContext dbContext)
        {
            InitializeComponent();
            _dbContext = dbContext; // DbContext ni konstruktor orqali oling
        }

        // "Add User" tugmasi bosilganda
        private async void AddUserButton_Click(object sender, RoutedEventArgs e)
        {
            // Foydalanuvchi ma'lumotlarini olish
            string fio = txtFIO.Text.Trim();
            string telefon = txtTelefon.Text.Trim();
            string adres = txtAdres.Text.Trim();
            string telegramRaqam = txtTelegramRaqam.Text.Trim();
            string innText = txtINN.Text.Trim();
            string okonx = txtOKONX.Text.Trim();
            string xisobRaqamText = txtXisobRaqam.Text.Trim();
            string jshshirText = txtJSHSHIR.Text.Trim();
            string bank = txtBank.Text.Trim();
            string firmaAdres = txtFirmaAdres.Text.Trim();

            // Majburiy maydonlarni tekshirish
            if (string.IsNullOrWhiteSpace(fio) ||
                string.IsNullOrWhiteSpace(telefon) ||
                string.IsNullOrWhiteSpace(adres) ||
                string.IsNullOrWhiteSpace(telegramRaqam) ||
                string.IsNullOrWhiteSpace(innText) ||
                string.IsNullOrWhiteSpace(okonx) ||
                string.IsNullOrWhiteSpace(xisobRaqamText) ||
                string.IsNullOrWhiteSpace(jshshirText) ||
                string.IsNullOrWhiteSpace(bank) ||
                string.IsNullOrWhiteSpace(firmaAdres))
            {
                MessageBox.Show("Iltimos, barcha maydonlarni to'ldiring.", "Xato", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // INN, Xisob Raqam va JSHSHIR qiymatlarini raqamga aylantirish
            if (!int.TryParse(innText, out int inn))
            {
                MessageBox.Show("INN faqat raqam bo'lishi kerak.", "Xato", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!int.TryParse(xisobRaqamText, out int xisobRaqam))
            {
                MessageBox.Show("Xisob Raqam faqat raqam bo'lishi kerak.", "Xato", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!int.TryParse(jshshirText, out int jshshir))
            {
                MessageBox.Show("JSHSHIR faqat raqam bo'lishi kerak.", "Xato", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Yangi foydalanuvchini yaratish
            NewUser = new User
            {
                FIO = fio,
                Telefon = telefon,
                Adres = adres,
                TelegramRaqam = telegramRaqam,
                INN = inn,
                OKONX = okonx,
                XisobRaqam = xisobRaqam.ToString(),
                JSHSHIR = jshshir.ToString(),
                Bank = bank,
                FirmaAdres = firmaAdres
            };

            try
            {
                // Yangi foydalanuvchini ma'lumotlar bazasiga qo'shish
                _dbContext.Users.Add(NewUser);
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
