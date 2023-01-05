using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juker.ViewModel
{
    internal class ProductViewModel : ViewModelBase 
    {

        private string _name;
        private string _category;

        public string Name
        {
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

        public string Category 
        {
            get
            {
                return _category;
            }

            set
            {
               _category = value;  
                OnPropertyChanged(nameof(Category));
            }
        }

     
    }
}
