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
        _customerPage.userDataGrid.Columns[1].Header = DeLong.WPF.Resources.Resource.Phone_;
        _customerPage.userDataGrid.Columns[2].Header = DeLong.WPF.Resources.Resource.Address_;
        _customerPage.userDataGrid.Columns[3].Header = DeLong.WPF.Resources.Resource.TelegramPhone_;
        _customerPage.userDataGrid.Columns[4].Header = DeLong.WPF.Resources.Resource.Tax_Identification_Number_;
        _customerPage.userDataGrid.Columns[5].Header = DeLong.WPF.Resources.Resource.Classifier_of_Enterprises_;
        _customerPage.userDataGrid.Columns[6].Header = DeLong.WPF.Resources.Resource.Bank__account_number;
        _customerPage.userDataGrid.Columns[7].Header = DeLong.WPF.Resources.Resource.JSHSHIR_;
        _customerPage.userDataGrid.Columns[8].Header = DeLong.WPF.Resources.Resource.Bank_;
        _customerPage.userDataGrid.Columns[9].Header = DeLong.WPF.Resources.Resource.Company_Address;
        _customerPage.userDataGrid.Columns[10].Header = DeLong.WPF.Resources.Resource.Action;

        languageComboBox.Text = DeLong.WPF.Resources.Resource.Language;
        btnMijoz.Content = DeLong.WPF.Resources.Resource.Customer;
        btnOmbor.Content = DeLong.WPF.Resources.Resource.Warehouse;
        btnMaxsulot.Content = DeLong.WPF.Resources.Resource.Product;
        btnKirim.Content = DeLong.WPF.Resources.Resource.Income;
        btnChiqim.Content = DeLong.WPF.Resources.Resource.Expense;
        btnChiqish.Content = DeLong.WPF.Resources.Resource.Exit;
        btnHisobot.Content = DeLong.WPF.Resources.Resource.Report;
        _customerPage.txtSearch.Text = DeLong.WPF.Resources.Resource.Search;
        _customerPage.btnAdd.Content = DeLong.WPF.Resources.Resource.Add;
        _customerPage.btnExcel.Content = DeLong.WPF.Resources.Resource.Export_to_Excel_;

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