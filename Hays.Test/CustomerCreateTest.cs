using FluentValidation;
using Hays.Domain.Entities;
using Hays.Domain.Abstraction.DataAcess;
using Hays.Domain.Abstraction.Repository;
using Hays.Domain.Services;
using Moq;
using System.Linq;
using System.Threading.Tasks;
using Hays.Domain.DTO;
using Hays.Domain.Validators;
using Xunit;

namespace Hays.Test
{
    public class CustomerCreateTest
    {
        private readonly Mock<IUnitOfWork> _mockUnitWork;
        Mock<ICustomersRepository> _customersRepository;
        private readonly CustomerValidator _customerValidator;
        public CustomerCreateTest()
        {
            _customersRepository = new Mock<ICustomersRepository>();
            _mockUnitWork = new Mock<IUnitOfWork>();
            _customerValidator = new CustomerValidator();
        }

        [Theory]
        [InlineData("", "", "", "", 4)]
        [InlineData("", "Moura", "anamoura@outlook.com", "123456", 1)]
        [InlineData("Ana", "", "anamoura@outlook.com", "123456", 1)]
        [InlineData("Aana", "Moura", "anamoura", "123456", 1)]
        [InlineData("Aana", "Moura", "anamoura@outlook.com", "", 1)]
        public async Task ShuldReturnValidationError(string name, string surname, string email, string password, int errosCount)
        {
            //Arrange
            var newCustomerDTO = new CustomerDTO { Email = email, Password = password, Surname = surname, Name = name, };
            var newCustomer = new Customers { Email = email, Password = password, Surname = surname, Name = name, };

            var validations = _customerValidator.Validate(newCustomer);

            Mock<IValidator<Customers>> validatorMock = new Mock<IValidator<Customers>>(MockBehavior.Strict);

            validatorMock
            .Setup(x => x.Validate(It.IsAny<Customers>()))
            .Returns(validations);

            var service = new CustomerService(_mockUnitWork.Object, validatorMock.Object);

            _mockUnitWork.Setup(uow => uow.CustomersRepository).Returns(_customersRepository.Object);

            //Act

            var result = await service.CretaeNewCustomer(newCustomerDTO);

            //Assert
            Assert.False(result.IsSuccess);

            Assert.True(result.Errors.Count() == errosCount);
        }

        [Theory]
        [InlineData("Aana", "Moura", "anamoura@outlook.com", "1234566", 1)]
        public async Task ShoudlReturnThatEmailAlreadyExists(string name, string surname, string email, string password, int errosCount)
        {
            //Arrange
            var newCustomerDTO = new CustomerDTO { Email = email, Password = password, Surname = surname, Name = name, };

            Mock<IValidator<Customers>> validatorMock = new Mock<IValidator<Customers>>(MockBehavior.Strict);

            validatorMock
            .Setup(x => x.Validate(It.IsAny<Customers>()))
            .Returns(new FluentValidation.Results.ValidationResult());

            var service = new CustomerService(_mockUnitWork.Object, validatorMock.Object);

            _customersRepository.Setup(x => x.CehckIfExistsByEmail(email))
               .ReturnsAsync(true);

            _mockUnitWork.Setup(uow => uow.CustomersRepository).Returns(_customersRepository.Object);

            //Act

            var result = await service.CretaeNewCustomer(newCustomerDTO);

            //Assert
            Assert.False(result.IsSuccess);

            Assert.True(result.Errors.Count() == errosCount);
        }

        [Fact]
        public async Task ShoudlReturnCreationSuccess()
        {
            //Arrange
            var newCustomerDTO = new CustomerDTO { Email = "anamoura@outlook.com", Password = "123456", Surname = "Moura", Name = "Aana", };

            Mock<IValidator<Customers>> validatorMock = new Mock<IValidator<Customers>>(MockBehavior.Strict);

            validatorMock
            .Setup(x => x.Validate(It.IsAny<Customers>()))
            .Returns(new FluentValidation.Results.ValidationResult());

            var service = new CustomerService(_mockUnitWork.Object, validatorMock.Object);

            _customersRepository.Setup(x => x.CehckIfExistsByEmail(newCustomerDTO.Email))
               .ReturnsAsync(false);

            _mockUnitWork.Setup(uow => uow.CustomersRepository).Returns(_customersRepository.Object);

            //Act

            var result = await service.CretaeNewCustomer(newCustomerDTO);

            //Assert
            Assert.True(result.IsSuccess);


        }

    }
}