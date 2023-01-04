using Juker.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Juker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static List<Product> Products = new List<Product>();
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Customer customer = new Customer();
            try
            {
                using (StreamReader file = File.OpenText(@"C:\Workspace\JUKER\Juker\Juker\data\products.json"))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    Products = (List<Product>)serializer.Deserialize(file, typeof(List<Product>));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
