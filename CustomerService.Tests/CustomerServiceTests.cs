using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Moq;
using Customer.Services;
using Customer.Repository;
using System.Collections.Generic;
using CustomerService.Exceptions;
using Customer.Dal;
using Customer.ViewModel;
using Customer.Common;

namespace CustomerService.Tests
{
    [TestClass]
    public class CustomerServiceTests
    {
        [TestFixture]
        public class When_creating_a_customer
        {
            //shows the basic arrange, act, assert pattern
            //shows the simple verification of a method call
            [Test]
            public void the_repository_save_should_be_called()
            {
                //Arrange
                var mockRepository = new Mock<ICustomerRepository>();

                mockRepository.Setup(x => x.Save(It.IsAny<Customer.ViewModel.Customer>()));

                var customerService = new CustomerServices(mockRepository.Object);

                //Act
                customerService.Create(new Customer.Dal.CustomerToCreateDTO());

                //Assert
                mockRepository.VerifyAll();
            }
             [Test]
            public void the_customer_repository_should_be_called_once_per_customer()
            {
                //Arrange
                var listofCustomerDTOS = new List<Customer.Dal.CustomerToCreateDTO>
               {
                   new Customer.Dal.CustomerToCreateDTO
                   {
                       City="Amsterdam",
                       Name="Amrita"
                   },
                   new Customer.Dal.CustomerToCreateDTO
                   {
                       City="Rotterdam",
                       Name="Samik"
                   }
               };

                var mockCustomerRepository = new Mock<ICustomerRepository>();
                mockCustomerRepository.Setup(x => x.Save(It.IsAny<Customer.ViewModel.Customer>()));
                var customerService = new CustomerServicemultiple(mockCustomerRepository.Object);

                //Act

                customerService.Create(listofCustomerDTOS);

                //Assert
                mockCustomerRepository.VerifyAll();
                mockCustomerRepository.Verify(x => x.Save(It.IsAny<Customer.ViewModel.Customer>())
                                             , Times.Exactly(listofCustomerDTOS.Count));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(CustomerService.Exceptions.InvalidCustomerMailingAddressException))]
        public void an_exception_should_be_thrown_if_the_address_is_not_created()
        {
            //Arrange
            var customerToCreateDto = new CustomerToCreateDTO
            { Name = "Bob", City = "Builder" };
            var mockAddressBuilder = new Mock<ICustomerAddressBuilder>();
            var mockCustomerRepository = new Mock<ICustomerRepository>();

            mockAddressBuilder
                .Setup(x => x.From(It.IsAny<CustomerToCreateDTO>()))
                .Returns(() => null);
            var customerService = new CustomerServiceDemo3(
                mockAddressBuilder.Object,
                mockCustomerRepository.Object);

            //Act
            customerService.Create(customerToCreateDto);

            //Assert
        }

        [TestMethod]
        public void customer_should_be_created_if_the_address_is_created()
        {
            //Arrange
            var customerToCreateDto = new CustomerToCreateDTO
            { Name = "Bob", City = "Builder" };
            var mockAddressBuilder = new Mock<ICustomerAddressBuilder>();
            var mockCustomerRepository = new Mock<ICustomerRepository>();

            mockAddressBuilder
                .Setup(x => x.From(It.IsAny<CustomerToCreateDTO>()))
                .Returns(() => new Customer.ViewModel.Address());

            var customerService = new CustomerServiceDemo3(
                mockAddressBuilder.Object,
                mockCustomerRepository.Object);

            //Act
            customerService.Create(customerToCreateDto);

            //Assert
            mockCustomerRepository
                .Verify(x => x.Save(It.IsAny<Customer.ViewModel.Customer>()));
        }

        [Test]
        public void the_customer_should_be_persisted()
        {
            //Arrange
            var mockCustomerRepository = new Mock<ICustomerRepository>();
            var mockMailingAddressFactory = new Mock<IMailingAddressFactory>();
            var mailingAddress = new MailingAddress { Country = "Netherlands" };
            mockMailingAddressFactory.Setup(x => x.TryParse(It.IsAny<string>(), out mailingAddress));
            var customerService = new CustomerserviceOutparam(
            mockCustomerRepository.Object, mockMailingAddressFactory.Object);

            //Act
            customerService.Create(new CustomerToCreateDTO());

            //Assert
            mockCustomerRepository.Verify(x => x.Save(It.IsAny<Customer.ViewModel.Customer>()));
        }

        [Test]
        public void each_customer_should_be_assigned_an_id()
        {
            //Arrange
            var listOfCustomersToCreate = new List<CustomerToCreateDTO>
                                                  {
                                                      new CustomerToCreateDTO(),
                                                      new CustomerToCreateDTO()
                                                  };

            var mockCustomerRepository = new Mock<ICustomerRepository>();
            var mockIdFactory = new Mock<IIdFactory>();
            var i = 1;
            mockIdFactory.Setup(x => x.Create())
                .Returns(() => i)
                .Callback(()=>i++);

            var customerService = new CustomerServiceMultipleReturnValues(
                mockCustomerRepository.Object, mockIdFactory.Object);

            //Act
            customerService.Create(listOfCustomersToCreate);

            //Assert
            mockIdFactory.Verify(x => x.Create(), Times.AtLeastOnce());
        }
        [Test]
        public void a_full_name_should_be_created_from_first_and_last_name()
        {
            //Arrange
            var customerToCreateDto = new CustomerToCreateDTO
            {
                Name = "Bob",
                City = "Builder"
            };

            var mockCustomerRepository = new Mock<ICustomerRepository>();
            var mockFullNameBuilder = new Mock<ICustomerFullNameBuilder>();

            mockFullNameBuilder.Setup(
                x => x.From(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(()=>customerToCreateDto.Name);

            var customerService = new CustomerServiceCorrectValuePassesThrough(
                mockCustomerRepository.Object, mockFullNameBuilder.Object);

            //Act
            customerService.Create(customerToCreateDto);

            //Assert
            mockFullNameBuilder.Verify(x => x.From
                                      (It.Is<string>(f => f.Equals(customerToCreateDto.Name, StringComparison.InvariantCulture)),
                                       It.Is<string>(f => f.Equals(customerToCreateDto.City, StringComparison.InvariantCulture))));
        }

        [Test]
        public void a_special_save_routine_should_be_used()
        {
            //Arrange
            var mockCustomerRepository = new Mock<ICustomerRepository>();
            var mockCustomerStatusFactory = new Mock<ICustomerStatusFactory>();

            var customerToCreate = new CustomerToCreateDTO
            {
                DesiredStatus = CustomerStatus.Platinum,
                Name = "Bob",
                City = "Builder"
            };

            mockCustomerStatusFactory.Setup(
                x => x.CreateFrom(
                    It.Is<CustomerToCreateDTO>(
                        y => y.DesiredStatus == CustomerStatus.Platinum)))
                        .Returns(CustomerStatus.Platinum);

            //mockCustomerStatusFactory.Setup(
            //    x => x.CreateFrom(
            //        It.Is<CustomerToCreateDTO>(
            //            y => y.DesiredStatus == CustomerStatus.Platinum)))
            //    .Returns(CustomerStatus.Platinum);


            var customerService = new CustomerServiceControlFlow(
                mockCustomerRepository.Object, mockCustomerStatusFactory.Object);

            //Act
            customerService.Create(customerToCreate);

            //Assert
            mockCustomerRepository.Verify(
                x => x.SaveSpecial(It.IsAny<Customer.ViewModel.Customer>()));
        }

        [Test]
        [ExpectedException(typeof(CustomerCreationException))]
        public void When_creating_a_customer_which_has_an_invalid_address_an_exception_should_be_raised()
        {
            //Arrange
            var mockCustomerRepository = new Mock<ICustomerRepository>();
            var mockCustomerAddressFactory = new Mock<ICustomerAddressFactory>();


            var customerService = new CustomerServicesRevised(
                mockCustomerRepository.Object,
                mockCustomerAddressFactory.Object);

            //Act
            customerService.Create(new CustomerToCreateDTO());

            //Assert
        }
    }
}
