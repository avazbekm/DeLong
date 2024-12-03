using System.Windows;
using DeLong.DbContexts;
using DeLong.Entities.Users;
using DeLong.Entities.Warehouses;
using Microsoft.EntityFrameworkCore;

namespace DeLong.Windows.Warehouses;

public partial class AddWarehouseWindow : Window
{
    private readonly AppdbContext _dbContext; // AppDbContext uchun private xususiyat
    public Warehouse NewWareHouse { get; private set; } // Yangi foydalanuvchi

    public AddWarehouseWindow(AppdbContext dbContext)
    {
        InitializeComponent();
        _dbContext = dbContext; // DbContext ni konstruktor orqali oling
    }

    // "Add User" tugmasi bosilganda
    private async void AddWarehouseButton_Click(object sender, RoutedEventArgs e)
    {
        // Foydalanuvchi ma'lumotlarini olish
        string id = txtWarehouseID.Text.Trim();
        string name = txtName.Text.Trim();
        string adres = txtAddress.Text.Trim();
        // Majburiy maydonlarni tekshirish
        if (string.IsNullOrWhiteSpace(name) ||
            string.IsNullOrWhiteSpace(id) ||
            string.IsNullOrWhiteSpace(adres))
        {
            MessageBox.Show("Iltimos, barcha maydonlarni to'ldiring.", "Xato", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        // INN, Xisob Raqam va JSHSHIR qiymatlarini raqamga aylantirish
        if (!int.TryParse(id, out int inn))
        {
            MessageBox.Show("INN faqat raqam bo'lishi kerak.", "Xato", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        
        // Yangi foydalanuvchini yaratish
        NewWareHouse = new Warehouse
        {
            Id=int.Parse(id),
            Name = name,
            Adres = adres,
            CreatedAt =DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };

        try
        {
            // Yangi foydalanuvchini ma'lumotlar bazasiga qo'shish
            _dbContext.Warehouses.Add(NewWareHouse);
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