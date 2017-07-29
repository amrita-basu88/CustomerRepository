using Customer.Dal;
using Customer.Repository;
using System.Collections;
using System.Collections.Generic;

namespace Customer.Services
{
    public class CustomerServicemultiple
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerServicemultiple(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }
        public void Create(IEnumerable<CustomerToCreateDTO> customersToCreate)
        {
            foreach (var customerToCreateDto in customersToCreate)
            {
                _customerRepository.Save(
                    new Customer.ViewModel.Customer(
                        customerToCreateDto.Name,
                        customerToCreateDto.City)
                    );
            }
        }
    }
}
