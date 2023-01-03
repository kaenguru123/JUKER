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
        string path = "C:\\Users\\Startklar\\Documents\\Git_Repos\\JUKER\\customer_data.json"; //individuell anpassen
        public Registration()
        {
            InitializeComponent();
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
            Customer newCustomer = new Customer();
            newCustomer.FirstName = "";
            newCustomer.LastName = "";
            newCustomer.PhoneNumber = "";
            newCustomer.Email = "";
            newCustomer.PictureUrl = "";
            newCustomer.Company = null;
            newCustomer.ProductIntrests = null;

            var initialJson = File.ReadAllText(FilePath);
            var existingCustomer = JArray.Parse(initialJson);
            existingCustomer.Add(newCustomer);
            var jsonToOutput = JsonConvert.SerializeObject(existingCustomer, Formatting.Indented);
        }
    }
}
