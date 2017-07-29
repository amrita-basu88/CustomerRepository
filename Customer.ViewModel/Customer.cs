
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customer.ViewModel
{
    public class Customer
    {
        public string Name { get; set; }
        public string City { get; set; }
        public Address Address { get; set; }
        public int Id { get; set; }

        public MailingAddress MailingAddress { get; set; }
        public Customer(string name, string city)
        {
            Name = name;
            City = city;
        }
    }
}
