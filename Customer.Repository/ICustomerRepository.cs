using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Customer.ViewModel;

namespace Customer.Repository
{
    public interface ICustomerRepository
    {
        void Save(Customer.ViewModel.Customer customer);
    }
}
