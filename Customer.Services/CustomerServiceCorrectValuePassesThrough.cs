using Customer.Dal;
using Customer.Repository;
using CustomerService.Exceptions;
using Customer.ViewModel;
using System.Collections.Generic;

namespace Customer.Services
{
    public class CustomerServiceCorrectValuePassesThrough
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ICustomerFullNameBuilder _customerFullName;

        public CustomerServiceCorrectValuePassesThrough(
            ICustomerRepository customerRepository,
            ICustomerFullNameBuilder customerFullName)
        {
            _customerRepository = customerRepository;
            _customerFullName = customerFullName;
        }

        public void Create(CustomerToCreateDTO customerToCreateDto)
        {
            var fullName = _customerFullName.From(
                customerToCreateDto.Name,
                customerToCreateDto.City);
            //Failed Condition
            //var fullName = _customerFullName.From(
            //    customerToCreateDto.Name,
            //    "");

            var customer = new Customer.ViewModel.Customer(fullName,"");

            _customerRepository.Save(customer);
        }
    }
}
