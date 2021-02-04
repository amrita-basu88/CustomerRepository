using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using Customer.ViewModel;

namespace Customer.Dal
{
    public class CustomerToCreateDTO
    {
        public string Name { get; set; }
        public string City { get; set; }
        public CustomerStatus DesiredStatus { get; set; }
    }
}
