using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Printing;
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
using Juker.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Juker.View
{
    /// <summary>
    /// Interaktionslogik für Registration.xaml
    /// </summary>
    public partial class Registration : UserControl
    {
        private readonly string path = @"C:\Users\Kenrick\Downloads\customer.json"; //individuell anpassen

        private List<Product> productList;
        private List<Product> customerInterests;
        private bool isCompanyCustomer;
        public Registration()
        {
            InitializeComponent();
            CompanyExtensionHead.Visibility = Visibility.Collapsed;
            CompanyExtension.Visibility = Visibility.Collapsed;

            productList = new List<Product>();
            customerInterests = new List<Product>();
            for (int i = 0; i < 15; i++)
            {
                productList.Add(new Product()
                {
                    Id = 0,
                    Name = "Bohrer",
                    Category = "Maschin"
                });

            }

            productList.Add(new Product()
            {
                Id = 0,
                Name = "Bohrer",
                Category = "Maschin"
            });
            productList.Add(new Product()
            {
                Id = 1,
                Name = "Wasch",
                Category = "Maschin"
            });

            ProductListView.ItemsSource = productList;
        }

        private void SubmitButtonClick(object sender, RoutedEventArgs e)
        {
            if (File.Exists(path))
            {
                InsertData(path);
            }
            else
            {
                File.Create(path).Close();
                InsertData(path);

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
                ProductIntrests = customerInterests
            };
            StreamReader r = new StreamReader(FilePath);
            string initialJson = r.ReadToEnd();
            r.Close();
            if (initialJson != "")
            {
                List<Customer>list = JsonConvert.DeserializeObject<List<Customer>>(initialJson);
                list.Add(newCustomer);
                var jsonToOutput = JsonConvert.SerializeObject(list, Formatting.Indented);
                File.WriteAllText(FilePath, jsonToOutput);
            }
            else
            {
                List<Customer> list = new List<Customer>();
                list.Add(newCustomer);
                string customerList = JsonConvert.SerializeObject(list, Formatting.Indented);
                File.WriteAllText(FilePath, customerList);
            }
            
            
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
