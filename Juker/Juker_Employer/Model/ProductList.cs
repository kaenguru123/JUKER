using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Juker.Model
{
    public class ProductList
    {
        private ProductList() { }
        public static void Initialize()
        {
            // Products = new List<Product>(//Datenbankabfrage);
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public static List<ProductList> Products { get; private set; }
    }
}
