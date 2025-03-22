using DeLong.WPF.Windows.Customers;
using System.Windows;
using System.Windows.Controls;

namespace DeLong.WPF.Pages.Customers;

/// <summary>
/// Interaction logic for CustomerPage.xaml
/// </summary>
public partial class CustomerPage : Page
{
    public CustomerPage()
    {
        InitializeComponent();
    }

    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
        CustomerAddWindow customerAddWindow = new CustomerAddWindow();
        customerAddWindow.ShowDialog();
    }
}
