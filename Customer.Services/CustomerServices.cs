using Customer.Dal;
using Customer.Repository;

namespace Customer.Services
{
    public class CustomerServices
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerServices(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public void Create(CustomerToCreateDTO customerToCreateDto)
        {
            var customer = BuildCustomerObjectFrom(customerToCreateDto);

            _customerRepository.Save(customer);
        }

        private Customer.ViewModel.Customer BuildCustomerObjectFrom(CustomerToCreateDTO customerToCreateDto)
        {
            return new Customer.ViewModel.Customer(customerToCreateDto.Name, customerToCreateDto.City);
        }

    }
}
