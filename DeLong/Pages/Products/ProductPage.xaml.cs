using System.Windows;
using System.Windows.Controls;
using DeLong.DbContexts;
using DeLong.Entities.Products;
using DeLong.Windows.Products;
using Microsoft.EntityFrameworkCore;

namespace DeLong.Pages.Products
{
    public partial class ProductPage : Page
    {
        private readonly AppdbContext _context;

        public ProductPage()
        {
            InitializeComponent();
            _context = new AppdbContext();
            LoadDataAsync();
        }
        private async Task LoadDataAsync()
        {
            var products = await _context.Products.ToListAsync();
            userDataGrid.ItemsSource = products;
        }
        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchText = txtSearch.Text.ToLower();

            var filteredUsers = await _context.Products
                .Where(u => u.Belgi.ToLower().Contains(searchText))
                .ToListAsync();

            userDataGrid.ItemsSource = filteredUsers; 
        }

        private async void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Product product)
            {
                var editWindow = new ProductEditWindow(_context, product); 

                if (editWindow.ShowDialog() == true)
                {
                    try
                    {
                        _context.Products.Update(editWindow.UpdatedProduct); 
                        await _context.SaveChangesAsync();
                        LoadDataAsync(); 
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Foydalanuvchini tahrirlashda xato: {ex.Message}");
                    }
                }
            }
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (userDataGrid.SelectedItem is Product selectedProduct)
            {
                _context.Products.Remove(selectedProduct);
                await _context.SaveChangesAsync();
                await LoadDataAsync(); 
            }
        }

        private void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
            var userForm = new AddProductWindow(_context); 
            if (userForm.ShowDialog() == true)
            {
                LoadDataAsync(); 
            }
        }
    }
}
