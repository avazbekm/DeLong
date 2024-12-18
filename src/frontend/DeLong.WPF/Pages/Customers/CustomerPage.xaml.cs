using System.Windows;
using System.Windows.Controls;
using DeLong.WPF.Windows.Customers;

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
