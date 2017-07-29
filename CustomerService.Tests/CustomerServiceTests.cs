﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Moq;
using Customer.Services;
using Customer.Repository;
using System.Collections.Generic;
using CustomerService.Exceptions;
using Customer.Dal;
using Customer.ViewModel;

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
    }
}