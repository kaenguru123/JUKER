using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Juker_Employer.Model
{
    internal class CustomerWithBlob : Customer
    {
        public Blob Photo { get; set; }
    }
}
