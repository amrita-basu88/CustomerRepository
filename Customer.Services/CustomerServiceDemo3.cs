 
using System.Collections.Generic;
using Customer.Dal;
using Customer.Repository;
using CustomerService.Exceptions;

namespace Customer.Services
{
   public class CustomerServiceDemo3
    {
        private readonly ICustomerAddressBuilder _customerAddressBuilder;
        private readonly ICustomerRepository _customerRepository;

        public CustomerServiceDemo3(ICustomerAddressBuilder customerAddressBuilder, ICustomerRepository customerRepository)
        {
            _customerAddressBuilder = customerAddressBuilder;
            _customerRepository = customerRepository;
        }

        public void Create(CustomerToCreateDTO customerToCreate)
        {
            var customer = new Customer.ViewModel.Customer(
                customerToCreate.Name,
                customerToCreate.City);

            customer.Address = _customerAddressBuilder.From(customerToCreate);

            if (customer.Address == null)
            {
                throw new InvalidCustomerMailingAddressException();
            }

            _customerRepository.Save(customer);
        }
    }
}
