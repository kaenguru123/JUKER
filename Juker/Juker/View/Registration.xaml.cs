using System;
using System.Collections.Generic;
using System.IO;
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
        string path = "A:Downloads/customer.json"; //individuell anpassen
        public Registration()
        {
            InitializeComponent();
            CompanyExtensionHead.Visibility = Visibility.Collapsed;
            CompanyExtension.Visibility = Visibility.Collapsed;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
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
            Company company = new Company
            {
                Name = CompanyName.Text,
                Street = CompanyStreet.Text,
                HouseNumber = CompanyHouseNumber.Text,
                City = CompanyCity.Text,
                Country = CompanyCountry.Text,
            };
            Customer newCustomer = new Customer()
            {
                FirstName = FirstName.Text,
                LastName = LastName.Text,
                PhoneNumber = PhoneNumber.Text,
                Email = Email.Text,
                PictureUrl = "",
                Company = company,
                ProductIntrests = null
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

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CompanyExtensionHead.Visibility = Visibility.Visible;
            CompanyExtension.Visibility = Visibility.Visible;
        }
        private void CheckBox_UnChecked(object sender, RoutedEventArgs e)
        {
            CompanyExtension.Visibility = Visibility.Collapsed;
            CompanyExtensionHead.Visibility = Visibility.Collapsed;
        }
    }
}
