using System.Windows;
using System.Windows.Controls;
using DeLong.DbContexts;
using DeLong.Entities.Users;
using DeLong.Windows.Users;
using Microsoft.EntityFrameworkCore;

namespace DeLong.Pages.Clients;

public partial class UserPage : Page
{
    private readonly AppdbContext _context;

    public UserPage()
    {
        InitializeComponent();
        _context = new AppdbContext(); 
        LoadUsers(); 
    }

    private async void LoadUsers()
    {
        try
        {
            var users = await _context.Users.ToListAsync(); 
            userDataGrid.ItemsSource = users; 
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Foydalanuvchilarni yuklashda xato: {ex.Message}");
        }
    }
    private async void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.DataContext is User user)
        {
            if (MessageBox.Show("Ushbu foydalanuvchini o'chirishni xohlaysizmi?", "O'chirish tasdiqlash", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    _context.Users.Remove(user);
                    await _context.SaveChangesAsync(); 
                    LoadUsers(); 
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Foydalanuvchini o'chirishda xato: {ex.Message}");
                }
            }
        }
    }
    private async void SearchButton_Click(object sender, RoutedEventArgs e)
    {
        string searchText = txtSearch.Text.ToLower();

        var filteredUsers = await _context.Users
            .Where(u => u.FIO.ToLower().Contains(searchText) || u.Telefon.Contains(searchText))
            .ToListAsync();

        userDataGrid.ItemsSource = filteredUsers; 
    }
    private void AddUserButton_Click(object sender, RoutedEventArgs e)
    {
        var userForm = new AddUserWindow(_context); 
        if (userForm.ShowDialog() == true)
        {
            LoadUsers(); 
        }
    }
    private async void EditButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.DataContext is User user)
        {
            var editWindow = new UserEditWindow(_context, user); 

            if (editWindow.ShowDialog() == true)
            {
                try
                {
                    _context.Users.Update(editWindow.UpdatedUser); 
                    await _context.SaveChangesAsync();
                    LoadUsers(); 
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Foydalanuvchini tahrirlashda xato: {ex.Message}");
                }
            }
        }
    }

}
