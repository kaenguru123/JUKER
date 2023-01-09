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
    public class ThankYouViewModel : ViewModelBase
    {
        public ICommand NavigateToWelcomePageCommand { get; }

        public ThankYouViewModel(NavigationStore navigationStore)
        {
            NavigateToWelcomePageCommand = new NavigateCommand<WelcomePageViewModel>(navigationStore, ()=> new WelcomePageViewModel(navigationStore));
        }
    }
}
