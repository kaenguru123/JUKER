using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using Juker.Model;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Juker.View
{
    /// <summary>
    /// Interaktionslogik für Registration.xaml
    /// </summary>
    public partial class Registration : UserControl
    {        
        private string pathProduct { get; set; }
        private string pathCustomer { get; set; }

        private List<Product> productList;
        private List<Product> customerInterests;
        private bool isCompanyCustomer;

        public Registration()
        {
            InitializeComponent();
            CompanyExtensionHead.Visibility = Visibility.Collapsed;
            CompanyExtension.Visibility = Visibility.Collapsed;

            customerInterests = new List<Product>();

            var downloadDirectory = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders", "{374DE290-123F-4565-9164-39C4925E467B}", String.Empty).ToString();

            var productDestDir = ConfigurationManager.AppSettings["product.json"] ?? "";
            pathProduct = downloadDirectory + productDestDir + "\\product.json";

            var customerDestDir = ConfigurationManager.AppSettings["customer.json"] ?? "";
            pathCustomer = downloadDirectory + customerDestDir + "\\customer.json";

            if (!(File.Exists(pathProduct)))
            {
                File.Create(pathProduct).Close();
            }
            try
            {
                var jsonDataProduct = File.ReadAllText(pathProduct);
                List<Product> productList = JsonConvert.DeserializeObject<List<Product>>(jsonDataProduct);
                ProductListView.ItemsSource = productList;
            }
            catch (Exception ex)
            {
                MessageBoxHelper.throwErrorMessageBox(
                    ex.Message + "\nNo products in customer.json or wrong path to customer.json.",
                    "Could not retrieve list of product from product.json");
            }
        }

        private void SubmitButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!(File.Exists(pathCustomer)))
                {
                    File.Create(pathCustomer).Close();
                }
                InsertData(pathCustomer);
            }
            catch (Exception ex)
            {
                MessageBoxHelper.throwErrorMessageBox(
                    ex.Message + "\ngive all mandatory information.",
                    "Invalid input");
            }
        }
        private void InsertData(string FilePath)
        {
            Company company = new Company();
            if (isCompanyCustomer)
            {
                company.Name = CompanyName.Text;
                company.Street = CompanyStreet.Text;
                company.HouseNumber = CompanyHouseNumber.Text;
                company.City = CompanyCity.Text;
                company.Country = CompanyCountry.Text;
            }

            Customer newCustomer = new Customer()
            {
                FirstName = FirstName.Text,
                LastName = LastName.Text,
                PhoneNumber = PhoneNumber.Text,
                Email = Email.Text,
                PictureUrl = "",
                Company = company,
                ProductInterests = customerInterests
            };
            StreamReader r = new StreamReader(FilePath);
            string initialJson = r.ReadToEnd();
            r.Close();
            
            var list = initialJson != "" ? JsonConvert.DeserializeObject<List<Customer>>(initialJson) : new List<Customer>();
            list.Add(newCustomer);
            string customerList = JsonConvert.SerializeObject(list, Formatting.Indented);
            File.WriteAllText(FilePath, customerList);
        }

        private void CompanyCheckBoxChecked(object sender, RoutedEventArgs e)
        {
            CompanyExtensionHead.Visibility = Visibility.Visible;
            CompanyExtension.Visibility = Visibility.Visible;
            isCompanyCustomer = true;
        }
        private void CompanyCheckBoxUnchecked(object sender, RoutedEventArgs e)
        {
            CompanyExtension.Visibility = Visibility.Collapsed;
            CompanyExtensionHead.Visibility = Visibility.Collapsed;
            isCompanyCustomer = false;
        }

        private void ProductCheckBoxChecked(object sender, RoutedEventArgs e)
        {
            if (sender != null)
            {
                CheckBox checkBox = (CheckBox)sender;
                Product selectedProduct = (Product)checkBox.DataContext;
                if (!customerInterests.Contains(selectedProduct))
                {
                    customerInterests.Add(selectedProduct);
                }
            }
        }

        private void ProductCheckBoxUnchecked(object sender, RoutedEventArgs e)
        {
            if (sender != null)
            {
                CheckBox checkBox = (CheckBox)sender;
                Product selectedProduct = (Product)checkBox.DataContext;
                customerInterests.Remove(selectedProduct);
            }
        }
    }
}
