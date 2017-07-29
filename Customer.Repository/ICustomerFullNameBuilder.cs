using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customer.Repository
{
    public interface ICustomerFullNameBuilder
    {
        string From(string firstName, string lastName);
    }
}
