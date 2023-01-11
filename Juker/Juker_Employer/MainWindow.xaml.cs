using Juker_Employer.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Juker_Employer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DbConnector DbConnection { get; set; }
        private string pathCustomer { get; set; }
        private string pathProduct { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            var downloadDirectory = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders", "{374DE290-123F-4565-9164-39C4925E467B}", String.Empty).ToString();

            var productDestDir = ConfigurationManager.AppSettings["product.json"] ?? "";
            pathProduct = downloadDirectory + productDestDir + "\\product.json";

            var customerDestDir = ConfigurationManager.AppSettings["customer.json"] ?? "";
            pathCustomer = downloadDirectory + customerDestDir + "\\customer.json";

            string ConnectionString = ConfigurationManager.AppSettings["connectionstring"];
            ConnectionString = ConnectionString ?? "server=localhost;database=juker;userid=root;";

            DbConnection = new DbConnector(ConnectionString);

            List<Customer> customers = DbConnection.getCustomerNameList();

            foreach (Customer customer in customers)
            {
                customer.ProductInterests = DbConnection.getCustomerInterestsById(customer.Id);
            }
            CustomerList.ItemsSource = customers;

        }

        private void CustomerList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CustomerList.SelectedItem != null)
            {
                Customer selectedCustomer = CustomerList.SelectedItem as Customer;
                ProductList.ItemsSource = selectedCustomer.ProductInterests;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            DbConnection.saveJsonToDatabase(pathCustomer);
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            DbConnection.updateProductJson(pathProduct);
        }
    }
}
