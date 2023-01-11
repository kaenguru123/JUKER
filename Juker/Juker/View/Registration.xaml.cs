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
using AForge.Video;
using AForge.Video.DirectShow;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Windows.Media.Imaging;
using static System.Net.Mime.MediaTypeNames;
using Image = System.Drawing.Image;
using Juker.ViewModel;

namespace Juker.View
{
    /// <summary>
    /// Interaktionslogik für Registration.xaml
    /// </summary>
    public partial class Registration : UserControl
    {
        #region Members
        private VideoCaptureDevice videoCaptureDevice;
        private FilterInfoCollection infoCollection;

        private string pathProduct { get; set; }
        private string pathCustomer { get; set; }

        private List<Product> productList;
        private List<Product> customerInterests;
        private bool isCompanyCustomer;
        private BitmapImage bitmapImage;
        private MemoryStream memoryStream;
        private string downloadDirectory = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders", "{374DE290-123F-4565-9164-39C4925E467B}", String.Empty).ToString();
        #endregion

        #region Constructor
        public Registration()
        {
            InitializeComponent();
            CompanyExtensionHead.Visibility = Visibility.Collapsed;
            CompanyExtension.Visibility = Visibility.Collapsed;
            SaveButton.Visibility = Visibility.Collapsed;

            customerInterests = new List<Product>();


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
        #endregion

        #region HelpMethods

        private void InsertData(string FilePath)
        {
            string newPath = "";
            Company company = new Company();
            if (isCompanyCustomer)
            {
                company.Name = CompanyName.Text;
                company.Street = CompanyStreet.Text;
                company.HouseNumber = CompanyHouseNumber.Text;
                company.City = CompanyCity.Text;
                company.Country = CompanyCountry.Text;
            }
            if (FirstName.Text != null || FirstName.Text != "")
            {
                FileInfo file = new FileInfo(downloadDirectory + "\\image.jpeg");
                string pictureFileName = FirstName.Text + DateTime.Now.ToString("M/d/yyyy");
                newPath = downloadDirectory + $"\\{pictureFileName}.jpeg";
                file.MoveTo(newPath); // hier könnte noch ein ,true hinter den newPath
            }
            Customer newCustomer = new Customer()
            {
                FirstName = FirstName.Text,
                LastName = LastName.Text,
                PhoneNumber = PhoneNumber.Text,
                Email = Email.Text,
                PhotoUrl = newPath,
                Company = company,
                ProductInterests = customerInterests
            };
            StreamReader reader = new StreamReader(FilePath);
            string initialJson = reader.ReadToEnd();
            reader.Close();
            var list = initialJson != "" ? JsonConvert.DeserializeObject<List<Customer>>(initialJson) : new List<Customer>();
            list?.Add(newCustomer);
            string customerList = JsonConvert.SerializeObject(list, Formatting.Indented);
            File.WriteAllText(FilePath, customerList);
        }
        private void FinaleFrame_newFrame(object sender, NewFrameEventArgs eventArgs)
        {
            try
            {
                using (var bitmap = (Bitmap)eventArgs.Frame.Clone())
                {
                    bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    memoryStream = new MemoryStream();
                    bitmap.Save(memoryStream, ImageFormat.Bmp);
                    bitmapImage.StreamSource = memoryStream;
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.EndInit();
                }
                bitmapImage.Freeze();
                Dispatcher.BeginInvoke(new ThreadStart(delegate
                { Webcam.Source = bitmapImage; }));
            }
            catch (Exception ex)
            {
                 MessageBoxHelper.throwErrorMessageBox(
                    ex.Message + "\nSomething went wrong with rendering",
                    "Rendering error");
            }
        }
        #endregion

        #region CheckBoxHandlers
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

        #endregion

        #region ButtonHandlers

        private void StartCamera(object sender, RoutedEventArgs e)
        {
            try
            {
                infoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                videoCaptureDevice = new VideoCaptureDevice(infoCollection[0].MonikerString);
                videoCaptureDevice.NewFrame += FinaleFrame_newFrame;
                videoCaptureDevice.Start();
                SaveButton.Visibility = Visibility.Visible;
                ImagePlaceHolder.Visibility = Visibility.Collapsed;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                MessageBoxHelper.throwErrorMessageBox(
                              "An error ccured while starting your camera. \nDo you have a camera installed?",
                               "Start camera error");
            }
            catch (Exception ex)
            {
                MessageBoxHelper.throwErrorMessageBox(
                 "An error ccured while starting your camera. Please try again. \n" + ex.Message,
                    "Start camera error");
            }
        }
        private void SaveImage(object sender, RoutedEventArgs e)
        {
            try
            {
                videoCaptureDevice.NewFrame -= FinaleFrame_newFrame;
                Image img = System.Drawing.Image.FromStream(memoryStream);
                videoCaptureDevice.SignalToStop();
                img.Save(downloadDirectory + "\\image.jpeg", ImageFormat.Jpeg);
                Webcam.Source = new BitmapImage(new Uri(downloadDirectory + "\\image.jpeg"));
                img.Dispose();
                SaveButton.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                MessageBoxHelper.throwErrorMessageBox(
                    ex.Message + "\nAn error ccured while capturing your image. Please take one again.",
                    "saving error");
            }
            finally
            {
                memoryStream.Dispose();
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
                ((RegistrationViewModel)DataContext).IsSaveable = false; 
            }
        }



        #endregion

        private void CommandBinding_CanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs e)
        {

        }
    }
}

