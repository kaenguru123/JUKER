using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Juker.Model
{
    public class Product
    {
        private Product() { }
        public static void Initialize()
        {
            // Products = new List<Product>(//Datenbankabfrage);
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        
    }
}
