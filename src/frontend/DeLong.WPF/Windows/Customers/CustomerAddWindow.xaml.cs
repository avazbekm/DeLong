using System.Windows;

namespace DeLong.WPF.Windows.Customers;

/// <summary>
/// Interaction logic for CustomerAddWindow.xaml
/// </summary>
public partial class CustomerAddWindow : Window
{
    public CustomerAddWindow()
    {
        InitializeComponent();
    }

    private void AddUserButton_Click(object sender, RoutedEventArgs e)
    {

    }

    private void rbtnYurdik_Checked(object sender, RoutedEventArgs e)
    {
        spYurCutomer.Visibility = Visibility.Visible;
        spJisCutomer.Visibility = Visibility.Hidden;
        spYattCutomer.Visibility = Visibility.Hidden;
    }

    private void rbtnYaTT_Checked(object sender, RoutedEventArgs e)
    {
        spYurCutomer.Visibility = Visibility.Hidden;
        spJisCutomer.Visibility = Visibility.Hidden;
        spYattCutomer.Visibility = Visibility.Visible;
    }

    private void rbtnJismoniy_Checked(object sender, RoutedEventArgs e)
    {

        spYurCutomer.Visibility = Visibility.Hidden;
        spJisCutomer.Visibility = Visibility.Visible;
        spYattCutomer.Visibility = Visibility.Hidden;

    }
}
