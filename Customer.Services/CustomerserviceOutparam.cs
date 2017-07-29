using System;
using System.Collections.Generic;
using Customer.Dal;
using Customer.Repository;
using CustomerService.Exceptions;
using Customer.ViewModel;

namespace Customer.Services
{
    public class CustomerserviceOutparam
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMailingAddressFactory _mailingAddressFactory;

        public CustomerserviceOutparam(ICustomerRepository customerRepository,
            IMailingAddressFactory mailingAddressFactory)
        {
            _customerRepository = customerRepository;
            _mailingAddressFactory = mailingAddressFactory;
        }

        public void Create(CustomerToCreateDTO customerToCreate)
        {
            var customer = new  Customer.ViewModel.Customer(customerToCreate.Name, customerToCreate.City);

            MailingAddress mailingAddress;
            var mailingAddressSuccessfullyCreated =
                _mailingAddressFactory.TryParse(
                   customerToCreate.City,
                   out mailingAddress);

            if (mailingAddress == null)
            {
                throw new InvalidCustomerMailingAddressException();
            }
            customer.MailingAddress = mailingAddress;
            _customerRepository.Save(customer);
        }
         
    }
}
