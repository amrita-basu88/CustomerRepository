using System;
using Customer.Dal;
using Customer.Repository;
using Customer.ViewModel;

namespace Customer.Services
{
    public class CustomerServiceControlFlow
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ICustomerStatusFactory _customerStatusFactory;
        public CustomerServiceControlFlow(ICustomerRepository customerRepository, ICustomerStatusFactory customerStatusFactory)
        {
            _customerRepository = customerRepository;
            _customerStatusFactory = customerStatusFactory;
        }

        public void Create(CustomerToCreateDTO customerToCreate)
        {
            var customer = new Customer.ViewModel.Customer(
                customerToCreate.Name, customerToCreate.City);

            customer.StatusLevel =
                _customerStatusFactory.CreateFrom(customerToCreate);

            if (customer.StatusLevel == CustomerStatus.Platinum)
            {
                _customerRepository.SaveSpecial(customer);
            }
            else
            {
                _customerRepository.Save(customer);
            }
        }
    }
}
