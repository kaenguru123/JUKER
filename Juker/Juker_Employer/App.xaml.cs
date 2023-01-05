using Juker.Model;
using Juker_Employer.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Juker_Employer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            //Datenbankabfrage

            DebugDbConnector();
            
        }

        private bool DebugDbConnector()
        {
            try
            {
                string pathCustomer = "C:\\Users\\Startklar\\Documents\\Git_Repos\\JUKER\\customer_data.json";
                string pathProduct = "C:\\Users\\Startklar\\Documents\\Git_Repos\\JUKER\\product.json";

                var connector = new DbConnector();

                List<Customer> customerList = connector.getCustomerNameList();

                Customer test = connector.getCustomerById(1);

                List<Product> testInterests = connector.getCustomerInterestsById(test.Id);
                test.ProductIntrests = testInterests;


                connector.saveJsonToDatabase(pathCustomer);

                connector.updateProductJson(pathProduct);

                return true;
            }
            catch (Exception)
            {
                throw;
                return false;
            }
        }
    }
}
