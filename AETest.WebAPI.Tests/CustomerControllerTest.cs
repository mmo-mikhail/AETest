using AETest.DataAccess;
using AETest.Domain;
using AETest.WebAPI.Controllers;
using AETest.WebAPI.Models.Request;
using AutoFixture;
using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace AETest.WebAPI.Tests
{
    public class CustomerControllerTest
    {
        [Theory]
        [InlineAutoMoqData(null)]
        [InlineAutoMoqData("")]
        [InlineAutoMoqData("   ")]
        public async Task Get_Error_BadRequest(
            string name,
            CustomerController customerController)
        {
            // Act
            var res = await customerController.Get(name);

            // Assert
            res.Should().NotBeNull();
            res.Result.Should().NotBeNull();
            res.Result.Should().BeOfType<BadRequestResult>();
        }

        [Theory]
        [AutoMoqData]
        public async Task Get_Error_NotFound_General(
            string name,
            [Frozen]Mock<IEntityRepository<Customer>> customerRepository,
            CustomerController customerController)
        {
            // Arrange
            Customer noCustomer = null;
            customerRepository
                .Setup(c => c.FindEntity(It.IsAny<Expression<Func<Customer, bool>>>()))
                .ReturnsAsync(noCustomer);

            // Act
            var res = await customerController.Get(name);

            // Assert
            res.Should().NotBeNull();
            res.Result.Should().NotBeNull();
            res.Result.Should().BeOfType<NotFoundResult>();
        }

        [Theory]
        [AutoMoqData]
        public async Task Get_Success_FirstName(
            string name,
            Customer customer,
            [Frozen]Mock<IEntityRepository<Customer>> customerRepository,
            CustomerController customerController)
        {
            // Arrange
            customer.FirstName = name;
            customerRepository
                .Setup(c => c.FindEntity(It.IsAny<Expression<Func<Customer, bool>>>()))
                .ReturnsAsync(customer);

            // Act
            var res = await customerController.Get(name);

            // Assert
            res.Should().NotBeNull();
            res.Result.Should().BeOfType<OkObjectResult>();
            var okRes = ((OkObjectResult)res.Result);
            okRes.StatusCode.Should().Be(200);
            okRes.Value.Should().BeOfType<CustomerModel>();
            var resModel = (CustomerModel)okRes.Value;

            resModel.Should().BeEquivalentTo(customer);
        }

        [Theory]
        [InlineAutoMoqData(-10)]
        [InlineAutoMoqData(-1)]
        [InlineAutoMoqData(0)]
        public async Task Delete_Error_BadRequest(
            int customerid,
            CustomerController customerController)
        {
            // Act
            var res = await customerController.Delete(customerid);

            // Assert
            res.Should().BeOfType<BadRequestResult>();
        }

        [Theory]
        [AutoMoqData]
        public async Task Delete_Error_NotFound(
            int customerid,
            [Frozen]Mock<IEntityRepository<Customer>> customerRepository,
            CustomerController customerController)
        {
            // Arrange
            Customer noCustomer = null;
            customerRepository
                .Setup(c => c.FindEntity(It.IsAny<Expression<Func<Customer, bool>>>()))
                .ReturnsAsync(noCustomer);

            // Act
            var res = await customerController.Delete(customerid);

            // Assert
            res.Should().BeOfType<NotFoundResult>();
        }

        [Theory]
        [AutoMoqData]
        public async Task Delete_Success(
            Customer customer,
            [Frozen]Mock<IEntityRepository<Customer>> customerRepository,
            CustomerController customerController)
        {
            // Arrange
            var customerId = customer.Id;
            customerRepository
                .Setup(c => c.FindEntity(It.IsAny<Expression<Func<Customer, bool>>>()))
                .ReturnsAsync(customer);

            // Act
            var res = await customerController.Delete(customerId);

            // Assert
            res.Should().NotBeNull();
            res.Should().BeOfType<OkResult>();
        }

        [Theory]
        [InlineAutoMoqData(null)]
        public async Task Post_Error_BadRequest_1(
            CustomerModel customerModel,
            CustomerController customerController)
        {
            // Act
            var res = await customerController.Post(customerModel);

            // Assert
            res.Should().NotBeNull();
            res.Should().BeOfType<BadRequestResult>();
        }

        [Theory]
        [AutoMoqData]
        public async Task Post_Error_BadRequest_InvalidModel(
            CustomerModel customerModel,
            CustomerController customerController)
        {
            //Arrange
            customerController.ModelState.AddModelError("An error", "Some error text");

            // Act
            var res = await customerController.Post(customerModel);

            // Assert
            res.Should().NotBeNull();
            res.Should().BeOfType<BadRequestResult>();
        }

        [Theory]
        [AutoMoqData]
        public async Task Post_Error_CustomerNotFound(
            CustomerModel targetCustomerModel,
            [Frozen]Mock<IEntityRepository<Customer>> customerRepository,
            CustomerController customerController)
        {
            //Arrange
            targetCustomerModel.Id = new Fixture().Create<int>();
            customerRepository
                .Setup(c => c.FindEntity(It.IsAny<Expression<Func<Customer, bool>>>()))
                .ReturnsAsync((Customer)null);

            // Act
            var res = await customerController.Post(targetCustomerModel);

            // Assert
            res.Should().NotBeNull();
            res.Should().BeOfType<NotFoundResult>();
        }

        [Theory]
        [AutoMoqData]
        public async Task Post_Success_CustomerAdded(
            CustomerModel targetCustomerModel,
            [Frozen]Mock<IEntityRepository<Customer>> customerRepository,
            CustomerController customerController)
        {
            //Arrange
            targetCustomerModel.Id = 0;
            customerRepository
                .Setup(c => c.Add(It.IsAny<Customer>()))
                .Callback<Customer>(c =>
                {
                    // Assert
                    c.Should().BeEquivalentTo(targetCustomerModel);
                });

            // Act
            var res = await customerController.Post(targetCustomerModel);

            // Assert
            res.Should().NotBeNull();
            res.Should().BeOfType<OkResult>();
            customerRepository.Verify(cr => cr.Add(It.IsAny<Customer>()), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task Post_Success_CustomerUpdated(
            CustomerModel targetCustomerModel,
            [Frozen]Mock<IEntityRepository<Customer>> customerRepository,
            CustomerController customerController)
        {
            //Arrange
            var customerDomain = new Fixture().Create<Customer>();
            targetCustomerModel.Id = customerDomain.Id;

            customerRepository
                .Setup(c => c.FindEntity(It.IsAny<Expression<Func<Customer, bool>>>()))
                .ReturnsAsync(customerDomain);

            customerRepository
                .Setup(c => c.Update(It.IsAny<Customer>()))
                .Callback<Customer>(c =>
                {
                    // Assert
                    c.Should().BeEquivalentTo(targetCustomerModel);
                });

            // Act
            var res = await customerController.Post(targetCustomerModel);

            // Assert
            res.Should().NotBeNull();
            res.Should().BeOfType<OkResult>();
            customerRepository.Verify(cr => cr.Update(It.IsAny<Customer>()), Times.Once);
        }
    }
}
