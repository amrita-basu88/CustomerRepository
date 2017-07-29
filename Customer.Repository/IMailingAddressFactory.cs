using Customer.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customer.Repository
{
    public interface IMailingAddressFactory
    {
        bool TryParse(string address, out MailingAddress mailingAddress);
    }
}
