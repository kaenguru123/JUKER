using Juker.Model;

using Juker_Employer.Model;
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
        private DbConnector _dbConnector = new DbConnector();
        private string pathBase = @"C:\Users\LDK2FE\Downloads\"; //individuell anpassen

        public MainWindow()
        {
            InitializeComponent();

            List<Customer> customers = _dbConnector.getCustomerNameList();

            foreach (Customer customer in customers)
            {
                customer.ProductInterests = _dbConnector.getCustomerInterestsById(customer.Id);
            }
            CustomerList.ItemsSource = customers;

            List<Product> productInterests = _dbConnector.getProducts();
            ProductList.ItemsSource = productInterests;
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
            _dbConnector.saveJsonToDatabase(pathBase + "customer.json");
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            _dbConnector.updateProductJson(pathBase + "product.json");
        }
    }
}
