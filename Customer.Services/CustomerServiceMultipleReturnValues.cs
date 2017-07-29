using Customer.Dal;
using Customer.Repository;
using CustomerService.Exceptions;
using Customer.ViewModel;
using System.Collections.Generic;

namespace Customer.Services
{
    public class CustomerServiceMultipleReturnValues
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IIdFactory _idFactory;

        public CustomerServiceMultipleReturnValues(
            ICustomerRepository customerRepository,
            IIdFactory idFactory)
        {
            _customerRepository = customerRepository;
            _idFactory = idFactory;
        }

        public void Create(IEnumerable<CustomerToCreateDTO> customersToCreate)
        {
            foreach (var customerToCreateDto in customersToCreate)
            {
                var customer = new Customer.ViewModel.Customer(
                    customerToCreateDto.Name,
                    customerToCreateDto.City);

                customer.Id = _idFactory.Create();

                _customerRepository.Save(customer);
            }
        }
    }
}
