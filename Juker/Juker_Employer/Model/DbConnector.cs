using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Markup;
using Juker_Employer.Model;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Utilities.Collections;

namespace Juker_Employer.Model
{
    internal class DbConnector
    {
        private MySqlConnection Connection { get; set; }
        private MySqlCommand Command { get; set; }
        public List<Company> CompanyList { get; set; }
        public List<Company> CompanyListWithoutIds { get; set; }
        public DbConnector(string ConnectionString)
        {
            try
            {
                Connection = new MySqlConnection();
                Connection.ConnectionString = ConnectionString;

                Command = new MySqlCommand();
                Command.Connection = Connection;

                Connection.Open();

                CompanyList = getCompanyList();
                CompanyListWithoutIds = deepCopy(CompanyList);
                foreach (Company company in CompanyListWithoutIds) { company.Id = -1; }
            }
            catch (MySqlException ex)
            {
                MessageBoxHelper.throwErrorMessageBox(
                    ex.Message + "\nCould not connect to database",
                    "Database connection failed");
            }
        }

        ~DbConnector()
        {
            Connection.Close();
        }
        public Customer getCustomerById(int customerId)
        {
            try
            {
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
            catch (Exception ex)
            {
                MessageBoxHelper.throwErrorMessageBox(
                    ex.Message + $"\nCould not retrieve customer with id={customerId}",
                    "Database connection failed");
                return new Customer();
            }
        }

        public List<Product> getProducts()
        {
            try { 
                string query = "SELECT " +
                                "product.sap_id, product.name, product.category " +
                                "FROM " +
                                "product;";

                Command.CommandText = query;
                var data = Command.ExecuteReader();

                List<Product> resultProducts = new List<Product>();
                while (data.Read())
                {
                    resultProducts.Add(getValidProduct(data));
                }

                data.Close();

                return resultProducts;
            }
            catch (Exception ex)
            {
                MessageBoxHelper.throwErrorMessageBox(
                    ex.Message + "\nCould not retrieve list of customer names.",
                    "Database connection failed");
                return new List<Product>();
            }
        }
        public List<Product> getCustomerInterestsById(int customerId)
        {
            try
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

                List<Product> resultProducts = new List<Product>();
                while (data.Read())
                {
                    resultProducts.Add(getValidProduct(data));
                }

                data.Close();

                return resultProducts;
            }
            catch (Exception ex)
            {
                MessageBoxHelper.throwErrorMessageBox(
                    ex.Message + "\nCould not retrieve list of customer names.",
                    "Database connection failed");
                return new List<Product>();
            }
        }
        public List<Customer> getCustomerNameList()
        {
            try
            {
                string query = "SELECT " +
                                "customer.customer_Id, customer.first_name, customer.last_name " +
                                "FROM " +
                                "customer;";

                Command.CommandText = query;
                var data = Command.ExecuteReader();

                List<Customer> resultNames = new List<Customer>();
                while (data.Read())
                {
                    resultNames.Add(getValidCustomer(data));
                }

                data.Close();

                return resultNames;
            }
            catch (Exception ex)
            {
                MessageBoxHelper.throwErrorMessageBox(
                    ex.Message + "\nCould not retrieve list of customer names.",
                    "Database connection failed");
                return new List<Customer>();
            }
        }

        public void saveJsonToDatabase(string path)
        {
            try
            {
                var jsonData = File.ReadAllText(path);

                List<Customer> newCustomers = JsonConvert.DeserializeObject<List<Customer>>(jsonData) ?? throw new Exception();
                foreach (Customer customer in newCustomers)
                {
                    saveCustomerToDatabase(customer);
                }
                File.Create(path).Close();
                MessageBoxHelper.throwSuccessMessageBox("Saved \"customer\" successfully to database");
            }
            catch (Exception ex)
            {
                MessageBoxHelper.throwErrorMessageBox(
                    ex.Message + "\nCould not save customer to Database!",
                    "Database connection failed");
            }
        }
        public void updateProductJson(string path)
        {
            try
            {
                string query = "SELECT " +
                                "* " +
                                "FROM " +
                                "product;";

                Command.CommandText = query;
                var data = Command.ExecuteReader();

                List<Product> resultProducts = new List<Product>();
                while (data.Read())
                {
                    resultProducts.Add(getValidProduct(data));
                }

                data.Close();

                var resultProductsInJson = JsonConvert.SerializeObject(resultProducts, Formatting.Indented);
                File.WriteAllText(path, resultProductsInJson);
                MessageBoxHelper.throwSuccessMessageBox("Updated \"product.json\" successfully");
            }
            catch (MySqlException ex)
            {
                MessageBoxHelper.throwErrorMessageBox(
                    ex.Message + "\nCould not retrieve products from Database!", 
                    "Database connection failed");
            }
        }

        private bool saveProductInterestToDatabase(int productId, int customerId)
        {
            string query = "INSERT INTO " +
                            "`product_customer` " +
                           "(`procu_id`, `sap_number`, `customer_id`) " +
                           $"VALUES(NULL, '{productId}', '{customerId}')";

            Command.CommandText = query;
            return Command.ExecuteNonQuery() > 0;
        }
        private bool saveCustomerToDatabase(Customer customerToSave)
        {
            int? companyId = null;
            if (customerToSave.Company != null && customerToSave.Company.Name != null)
            {
                companyId = trySaveCompanyToDatabaseIfNotExisting(customerToSave.Company);
            }
            

         
            string companyIdAsString = companyId!=null ? companyId.ToString() : "NULL";
            string query = "INSERT INTO " +
                            "`customer` " +
                            "(`customer_id`, `first_name`, `last_name`, `phone_number`, `email`, `photo_url`, `company`) " +
                            $"VALUES(NULL, '{customerToSave.FirstName}', '{customerToSave.LastName}', '{customerToSave.PhoneNumber}', '{customerToSave.Email}', '{customerToSave.PhotoUrl}', {companyIdAsString});";

            Command.CommandText = query;
            if (Command.ExecuteNonQuery() == 0) return false;
            int customerId = getLastInsertedId();
            
            if (customerToSave.ProductInterests != null && customerToSave.ProductInterests.Count != 0) { 
                foreach (Product product in customerToSave.ProductInterests)
                {
                    saveProductInterestToDatabase(product.Id, customerId);
                }
            }
            return true;
        }
        private int saveCompanyToDatabase(Company companyToSave)
        {
            string companyQuery = "INSERT INTO " +
                                    "`company` " +
                                    "(`company_id`, `name`, `street`, `house_number`, `city`, `country`) " +
                                    $"VALUES (NULL, '{companyToSave.Name}', '{companyToSave.Street}', '{companyToSave.HouseNumber}', '{companyToSave.City}', '{companyToSave.Country}')";
            Command.CommandText = companyQuery;
            var res = Command.ExecuteNonQuery();

            return getLastInsertedId();
        }
        private int trySaveCompanyToDatabaseIfNotExisting(Company companyToSave)
        {
            int companyIdIfAlreadyExisting = getExistingCompanyId(companyToSave);
            if (companyIdIfAlreadyExisting != -1)
            {
                return companyIdIfAlreadyExisting;
            }

            return saveCompanyToDatabase(companyToSave);
        }
        private List<Company> getCompanyList()
        {
            try
            {
                string query = "SELECT " +
                                "* " +
                                "FROM " +
                                "company;";

                Command.CommandText = query;
                var data = Command.ExecuteReader();

                var resultCompanies = new List<Company>();
                while (data.Read())
                {
                    resultCompanies.Add(getValidCompany(data));
                }

                data.Close();

                return resultCompanies;
            }
            catch (Exception ex)
            {
                MessageBoxHelper.throwErrorMessageBox(
                    ex.Message + "\nCould not retrieve products from Database!",
                    "Database connection failed");
                return new List<Company>();
            }
        }
        private int getExistingCompanyId(Company companyToCompare)
        {
            foreach (Company company in CompanyList)
            {
                if (company.EqualsWithoutId(companyToCompare))
                {
                    return company.Id;
                }
            }
            return -1;
        }
        private static bool tryIndex(MySqlDataReader data, string key)
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
        private static Company getValidCompany(MySqlDataReader data)
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
        private static Customer getValidCustomer(MySqlDataReader data)
        {
            if (data == null) { return null; }
            var validCustomer = new Customer();

            validCustomer.Id = tryIndex(data, "customer_id") ? Int32.Parse(data["customer_id"].ToString()) : default(int);
            validCustomer.FirstName = tryIndex(data, "first_name") ? data["first_name"].ToString() : default(string);
            validCustomer.LastName = tryIndex(data, "last_name") ? data["last_name"].ToString() : default(string);
            validCustomer.PhoneNumber = tryIndex(data, "phone_number") ? data["phone_number"].ToString() : default(string);
            validCustomer.Email = tryIndex(data, "email") ? data["email"].ToString() : default(string);
            validCustomer.PhotoUrl = tryIndex(data, "photo_url") ? data["photo_url"].ToString() : default(string);
            int CompanyId = tryIndex(data, "company") ? Int32.Parse(data["company"].ToString()) : default(int);
            if (CompanyId != default(int))
            {
                validCustomer.Company = getValidCompany(data);
            }

            return validCustomer;
        }
        private static Product getValidProduct(MySqlDataReader data)
        {
            if (data == null) { return null; }
            var validProduct = new Product();

            validProduct.Id = tryIndex(data, "sap_id") ? Int32.Parse(data["sap_id"].ToString()) : default(int);
            validProduct.Name = tryIndex(data, "name") ? data["name"].ToString() : default(string);
            validProduct.Category = tryIndex(data, "category") ? data["category"].ToString() : default(string);

            return validProduct;
        }
        private int getLastInsertedId()
        {
            string lastIdQuery = "SELECT LAST_INSERT_ID() as Id;";
            Command.CommandText = lastIdQuery;
            var data = Command.ExecuteReader();
            data.Read();
            int id = Int32.Parse(data["Id"].ToString());
            data.Close();
            return id;
        }
        private static T deepCopy<T>(T originObject)
        {
            var result = JsonConvert.SerializeObject(originObject);
            return JsonConvert.DeserializeObject<T>(result);
        }
    }
}
