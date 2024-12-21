using System.Windows;
using System.Globalization;
using System.Windows.Controls;
using DeLong.WPF.Pages.Customers;


namespace DeLong.WPF;

#pragma warning disable
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private CustomerPage _customerPage;
    public MainWindow()
    {
        InitializeComponent();
    }

    private string _currentLanguage = "en"; // Tanlangan tilni saqlash uchun o'zgaruvchi

    private void LanguageAPP(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        if (languageComboBox.SelectedItem is ComboBoxItem selectedItem)
        {
            string selectedLanguage = selectedItem.Tag.ToString();

            if (_currentLanguage != selectedLanguage) // Faqat til o'zgarishida yangilash
            {
                _currentLanguage = selectedLanguage;
                DeLong.WPF.Resources.Resource.Culture = new CultureInfo(selectedLanguage);
                UpdateLanguage(); // Matnlarni yangilash
            }
        }
    }
    private void UpdateLanguage()
    {
        if (_customerPage == null)
        {
            _customerPage = new CustomerPage();
        }
        _customerPage.userDataGrid.Columns[0].Header= DeLong.WPF.Resources.Resource.FirstName;

    }

    private void bntMijoz_IsMouseDirectlyOverChanged(object sender, DependencyPropertyChangedEventArgs e)
    {

    }

    private void btnChiqish_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
    }

    private void bntMijoz_Click(object sender, RoutedEventArgs e)
    {
        // UserPage sahifasini yaratish yoki ochish
        if (_customerPage == null)
        {
            _customerPage = new CustomerPage(); // UserPage faqat bir marta yaratiladi
        }

        // Tanlangan tilni qo'llash
        DeLong.WPF.Resources.Resource.Culture = new CultureInfo(_currentLanguage); // Tanlangan tilni qo'llash
        UpdateLanguage(); // Matnlarni yangilash

        // UserPage sahifasiga o'tish
        Navigator.Navigate(_customerPage);
    }
}