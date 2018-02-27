using Customer.Dal;
using Customer.ViewModel;

namespace Customer.Repository
{
    public interface ICustomerAddressFactory
    {
        Address From(CustomerToCreateDTO customerToCreate);
    }
}