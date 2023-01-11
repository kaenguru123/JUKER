using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Juker_Employer
{
    public static class MessageBoxHelper
    {
        public static void throwErrorMessageBox(string message, string caption)
        {
            MessageBox.Show(message, "ERROR! " + caption,
                                 MessageBoxButton.OK,
                                 MessageBoxImage.Error);
        }
    }
}
