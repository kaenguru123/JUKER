using Juker.Model;
using Juker.Stores;
using Juker.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Juker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Juker : Window
    {
        public Juker()
        {
            NavigationStore navigationStore = new NavigationStore();
            navigationStore.CurrentViewModel = new WelcomePageViewModel(navigationStore);
            InitializeComponent();
            DataContext = new MainViewModel(navigationStore);
        }

    }
}
