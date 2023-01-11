using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Juker
{
    public static class MessageBoxHelper
    {
        public static void throwErrorMessageBox(string caption, string message)
        {
            MessageBox.Show(message, "ERROR!" + caption,
                                 MessageBoxButton.OK,
                                 MessageBoxImage.Error);
            Environment.Exit(-1);
        }
    }
}
