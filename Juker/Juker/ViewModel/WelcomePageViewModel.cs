using Juker.Commands;
using Juker.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Juker.ViewModel
{
    public class WelcomePageViewModel : ViewModelBase
    {
        public ICommand NavigateToRegistrationCommand { get; }

        public WelcomePageViewModel(NavigationStore navigationStore)
        {
            NavigateToRegistrationCommand = new NavigateCommand<RegistrationViewModel>(navigationStore, () => new RegistrationViewModel(navigationStore));
        }
    }
}
