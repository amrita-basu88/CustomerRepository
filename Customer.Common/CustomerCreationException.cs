using System;

namespace Customer.Common
{
    public class CustomerCreationException : Exception
    {
        public CustomerCreationException(Exception exception) : base("error", exception)
        {

        }
    }
}
