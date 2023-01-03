using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Juker.ViewModel
{
    internal class CompanyViewModel : ViewModelBase
    {

        public ICommand MakeCompanyRegistration { get; }

        public CompanyViewModel()
        {

        }


        private string _name;
        public string Name { 
        
        get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

  
        private string _street;
        public string Street
        {

            get
            {
                return _street;
            }
            set
            {
                _street = value;
                OnPropertyChanged(nameof(Street));
            }
        }

        private string _houseNumber;
        public string HouseNumber
        {

            get
            {
                return _houseNumber;
            }
            set
            {
                _houseNumber = value;
                OnPropertyChanged(nameof(HouseNumber));
            }
        }
        
        private string _city;
        public string City
        {

            get
            {
                return _city;
            }
            set
            {
                _city = value;
                OnPropertyChanged(nameof(City));
            }
        }

        private string _country;
        public string Country
        {

            get
            {
                return _country;
            }
            set
            {
                _country = value;
                OnPropertyChanged(nameof(Country));
            }
        }

    }
}
