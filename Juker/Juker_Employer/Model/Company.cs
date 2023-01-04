using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juker.Model
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

        public bool EqualsWithoutId(Company companyToCompare)
        {
            return (Name.ToLower() == companyToCompare.Name.ToLower() &&
                   Street.ToLower() == companyToCompare.Street.ToLower() &&
                   HouseNumber.ToLower() == companyToCompare.HouseNumber.ToLower() &&
                   City.ToLower() == companyToCompare.City.ToLower() &&
                   Country.ToLower() == companyToCompare.Country.ToLower());
        }
    }
}
