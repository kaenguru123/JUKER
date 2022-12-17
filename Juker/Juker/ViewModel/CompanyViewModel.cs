using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juker.ViewModel
{
    internal class CompanyViewModel : ViewModelBase
    {
        private string _firstName;
        public string FirstName { 
        
        get
            {
                return _firstName;
            }
            set
            {
                _firstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }

        private string _lastName;
        public string LastName
        {

            get
            {
                return _lastName;
            }
            set
            {
                _lastName = value;
                OnPropertyChanged(nameof(LastName));
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
