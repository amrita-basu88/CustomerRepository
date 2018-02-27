using Customer.Common;
using Customer.Dal;
using Customer.Repository;

namespace Customer.Services
{
    public class CustomerServicesRevised
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ICustomerAddressFactory _customerAddressFactory;

        public CustomerServicesRevised(ICustomerRepository customerRepository, ICustomerAddressFactory customerAddressFactory)
        {
            _customerRepository = customerRepository;
            _customerAddressFactory = customerAddressFactory;
        }

        public void Create(CustomerToCreateDTO customerToCreate)
        {
            
                var customer = new Customer.ViewModel.Customer(customerToCreate.Name,null);
                customer.Address = _customerAddressFactory.From(customerToCreate);
                if (customer.Address == null)
                {
                    throw new InvalidCustomerAddressException();
                }

                _customerRepository.Save(customer); 
        }
    }
}
