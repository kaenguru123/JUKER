using Juker.Commands;
using Juker.Model;
using Juker.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Juker.ViewModel
{
    public class RegistrationViewModel : ViewModelBase
    {
        public ICommand NavigateToWelcomePageCommand { get; }
        public ICommand NavigateToThankYouPageCommand { get;  }

        public RegistrationViewModel(NavigationStore navigationStore)
        {
            NavigateToWelcomePageCommand = new NavigateCommand<WelcomePageViewModel>(navigationStore, () => new WelcomePageViewModel(navigationStore));
            NavigateToThankYouPageCommand = new NavigateCommand<ThankYouViewModel>(navigationStore, () => new ThankYouViewModel(navigationStore));
        }



    }
}
