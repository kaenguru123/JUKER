using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using Juker.Model;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace Juker_Employer.Model
{
    internal class DbConnector
    {
        private MySqlConnection Connection { get; set; }
        private MySqlCommand Command { get; set; }
        public DbConnector()
        {
            try
            {
                string ConnectionString = "server=localhost;database=juker;userid=root;";
                Connection = new MySqlConnection();
                Connection.ConnectionString = ConnectionString;

                Command = new MySqlCommand();
                Command.Connection = Connection;

                Connection.Open();
            }
            catch (MySqlException err)
            {
                MessageBox.Show(err.Message, "ERROR! Connection to database failed!", 
                                MessageBoxButton.OK, 
                                MessageBoxImage.Error);
                Environment.Exit(-1);
            }
        }

        ~DbConnector()
        {
            Connection.Close();
        }

        private bool tryIndex(MySqlDataReader data, string key)
        {
            if (data == null) { return false; }
            try
            {        
                var tryIndex = data[key];
                return true;
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }
        }

        private Company getValidCompanyById(MySqlDataReader data)
        {
            var validCompany = new Company();

            validCompany.Id = tryIndex(data, "company_id") ? Int32.Parse(data["company_id"].ToString()) : default(int);
            validCompany.Name = tryIndex(data, "name") ? data["name"].ToString() : default(string);
            validCompany.Street = tryIndex(data, "street") ? data["street"].ToString() : default(string);
            validCompany.HouseNumber = tryIndex(data, "house_number") ? data["house_number"].ToString() : default(string);
            validCompany.City = tryIndex(data, "city") ? data["city"].ToString() : default(string);
            validCompany.Country = tryIndex(data, "country") ? data["country"].ToString() : default(string);

            return validCompany;
        }

        private Customer getValidCustomer(MySqlDataReader data)
        {
            if (data == null) { return null; }
            var validCustomer = new Customer();

            validCustomer.Id = tryIndex(data, "customer_id") ? Int32.Parse(data["customer_id"].ToString()) : default(int);
            validCustomer.FirstName = tryIndex(data, "first_name") ? data["first_name"].ToString() : default(string);
            validCustomer.LastName = tryIndex(data, "last_name") ? data["last_name"].ToString() : default(string);
            validCustomer.PhoneNumber = tryIndex(data, "phone_number") ? data["phone_number"].ToString() : default(string);
            validCustomer.Email = tryIndex(data, "email") ? data["email"].ToString() : default(string);
            validCustomer.PictureUrl = tryIndex(data, "picture_url") ? data["picture_url"].ToString() : default(string);
            int CompanyId = tryIndex(data, "company") ? Int32.Parse(data["company"].ToString()) : default(int);
            if (CompanyId != default(int))
            {
                validCustomer.Company = getValidCompanyById(data);
            }

            return validCustomer;
        }
        private Product getValidProduct(MySqlDataReader data)
        {
            if (data == null) { return null; }
            var validProduct = new Product();

            validProduct.Id = tryIndex(data, "sap_id") ? Int32.Parse(data["sap_id"].ToString()) : default(int);
            validProduct.Id = tryIndex(data, "sap_number") ? Int32.Parse(data["sap_number"].ToString()) : default(int);
            validProduct.Name = tryIndex(data, "name") ? data["name"].ToString() : default(string);
            validProduct.Category = tryIndex(data, "category") ? data["category"].ToString() : default(string);

            return validProduct;
        }



        private List<Product> getInterestList(MySqlDataReader data)
        {
            var list = new List<Product>();
            while (data.Read())
            {
                list.Add(getValidProduct(data));
            }
            return list;
        }

        private List<Customer> getCustomerList(MySqlDataReader data)
        {
            var list = new List<Customer>();
            while (data.Read())
            {
                list.Add(getValidCustomer(data));
            }
            return list;
        }

        public Customer getCustomerById(int customerId)
        {
            //SELECT* FROM customer LEFT OUTER JOIN company ON company.company_id = customer.company LEFT OUTER JOIN product_customer ON product_customer.customer_id = customer.customer_id LEFT OUTER JOIN product ON product.sap_id = product_customer.sap_number;
     
            //SELECT* FROM customer LEFT OUTER JOIN company ON company.company_id = customer.company;
            string query = "SELECT " +
                            "* " +
                            "FROM " +
                            "`customer` " +
                            "LEFT OUTER JOIN " +
                            "company " +
                            "ON " +
                            "customer.company = company.company_id " +
                            "WHERE " + 
                            "customer.customer_id = " +
                            customerId + ";";
            
            Command.CommandText = query;
            var data = Command.ExecuteReader();
            data.Read();
            Customer resultCustomer = getValidCustomer(data);
            data.Close();
            return resultCustomer;
        }

        public List<Product> getCustomerInterestsById(int customerId)
        {
            string query = "SELECT " +
                            "product.sap_id, product.name, product.category " +
                            "FROM " +
                            "product_customer " +
                            "LEFT OUTER JOIN " +
                            "product " +
                            "ON " +
                            "product_customer.sap_number = product.sap_id " +
                            "WHERE " +
                            "product_customer.customer_id = " +
                            customerId + ";";

            Command.CommandText = query;
            var data = Command.ExecuteReader();
            List<Product> resultProducts = getInterestList(data);
            data.Close();
            return resultProducts;
        }

        public List<Customer> getCustomerNameList()
        {
            string query = "SELECT " +
                            "customer.customer_Id, customer.first_name, customer.last_name " +
                            "FROM " +
                            "customer;";

            Command.CommandText = query;
            var data = Command.ExecuteReader();
            List<Customer> resultNames = getCustomerList(data);
            data.Close();
            return resultNames;
        }
    }
}
