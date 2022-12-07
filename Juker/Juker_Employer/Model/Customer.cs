﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Juker.Model
{
    internal class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string PictureUrl { get; set; }
        public Company Company { get; set; }
        public List<ProductList> ProductIntrests { get; set; }
    }
}