using Juker.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Juker.ViewModel
{
    internal class CustomerViewModel : ViewModelBase
    {
        private string _firstName;
        private string _lastName;
        private string _phoneNumber;
        private string _email;
        private string _pictureUrl;
        private Company _company;
        private List<Product> _productInterests;

        public string FirstName
        {
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
        public string PhoneNumber
        {
            get
            {
                return _phoneNumber;
            }
            set
            {
                _phoneNumber = value;
                OnPropertyChanged(nameof(PhoneNumber));
            }

        }
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }

        }
        public string PictureUrl
        {
            get
            {
                return _pictureUrl;
            }
            set
            {
                _pictureUrl = value;
                OnPropertyChanged(nameof(PictureUrl));
            }

        }
        public Company Company
        {
            get
            {
                return _company;
            }
            set
            {
                _company = value;
                OnPropertyChanged(nameof(Company));
            }

        }
        public List<Product> ProductIntrests
        {
            get
            {
                return _productInterests;
            }
            set
            {
                _productInterests = value;
                OnPropertyChanged(nameof(ProductIntrests));
            }

        }


    }
}
