using FluentValidation;
using Hays.Domain.Entities;
using Hays.Domain.Abstraction.DataAcess;
using Hays.Domain.Abstraction.Repository;
using Hays.Domain.Services;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using Hays.Domain.DTO;
using Hays.Domain.Validators;
using Xunit;

namespace Hays.Test
{
    public class CustomerUpdateTest
    {
        private readonly Mock<IUnitOfWork> _mockUnitWork;
        private readonly Mock<ICustomersRepository> _customersRepository;
        private readonly CustomerValidator _customerValidator;
        private readonly Guid _customerId;
        public CustomerUpdateTest()
        {
            _customerId = new Guid("A5527163-D9F8-4241-98E9-907BEFCCF117");

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

            var result = await service.UpdateCustomer(newCustomerDTO, _customerId);

            //Assert
            Assert.False(result.IsSuccess);

            Assert.True(result.Errors.Count() == errosCount);
        }

        [Fact]
        public async Task ShoudlReturnThatCustomerNotExists()
        {
            //Arrange
            var newCustomerDTO = new CustomerDTO { Email = "anamoura@outlook.com", Password = "123456", Surname = "Moura", Name = "Aana", };

            Mock<IValidator<Customers>> validatorMock = new Mock<IValidator<Customers>>(MockBehavior.Strict);

            validatorMock
            .Setup(x => x.Validate(It.IsAny<Customers>()))
            .Returns(new FluentValidation.Results.ValidationResult());

            _customersRepository.Setup(x => x.GetByIdToEdit(_customerId))
                .ReturnsAsync((Customers?)null);


            var service = new CustomerService(_mockUnitWork.Object, validatorMock.Object);

            _mockUnitWork.Setup(uow => uow.CustomersRepository).Returns(_customersRepository.Object);

            //Act

            var result = await service.UpdateCustomer(newCustomerDTO, _customerId);

            //Assert
            Assert.False(result.IsSuccess);

            Assert.True(result.Errors.Count() > 0);
        }

        [Fact]
        public async Task ShoudlReturnThatEmailIsAlreadyBeingUsedForAnotherCustomer()
        {
            //Arrange
            var newCustomerDTO = new CustomerDTO { Email = "anamoura@outlook.com", Password = "123456", Surname = "Moura", Name = "Aana" };

            Mock<IValidator<Customers>> validatorMock = new Mock<IValidator<Customers>>(MockBehavior.Strict);

            validatorMock
            .Setup(x => x.Validate(It.IsAny<Customers>()))
            .Returns(new FluentValidation.Results.ValidationResult());

            _customersRepository.Setup(x => x.GetByIdToEdit(_customerId))
                .ReturnsAsync(new Customers { Email = "anamoura@outlook.com", Password = "123456", Surname = "Moura", Name = "Aana" });
            _customersRepository.Setup(x => x.GetById(_customerId))
                .ReturnsAsync(new CustomerViewDTO { Email = "anamoura@outlook.com", Surname = "Moura", Name = "Aana" });

            _customersRepository.Setup(x => x.CehckIfExistsByEmail(newCustomerDTO.Email))
               .ReturnsAsync(true);

            _customersRepository.Setup(x => x.GetEmailById(_customerId))
                .ReturnsAsync("marcosa@outlook.com");


            var service = new CustomerService(_mockUnitWork.Object, validatorMock.Object);

            _mockUnitWork.Setup(uow => uow.CustomersRepository).Returns(_customersRepository.Object);

            //Act

            var result = await service.UpdateCustomer(newCustomerDTO, _customerId);

            //Assert
            Assert.False(result.IsSuccess);

            Assert.True(result.Errors.Count() > 0);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task ShoudlReturnUdpateSuccess(bool isEmailRegistrated)
        {
            //Arrange
            var newCustomerDTO = new CustomerDTO { Email = "anamoura@outlook.com", Password = "123456", Surname = "Moura", Name = "Aana" };

            Mock<IValidator<Customers>> validatorMock = new Mock<IValidator<Customers>>(MockBehavior.Strict);

            validatorMock
            .Setup(x => x.Validate(It.IsAny<Customers>()))
            .Returns(new FluentValidation.Results.ValidationResult());

            _customersRepository.Setup(x => x.GetByIdToEdit(_customerId))
                .ReturnsAsync(new Customers { Email = "anamoura@outlook.com", Password = "123456", Surname = "Moura", Name = "Aana" });
            _customersRepository.Setup(x => x.GetById(_customerId))
                .ReturnsAsync(new CustomerViewDTO { Email = "anamoura@outlook.com", Surname = "Moura", Name = "Aana" });

            _customersRepository.Setup(x => x.CehckIfExistsByEmail(newCustomerDTO.Email))
               .ReturnsAsync(isEmailRegistrated);

            _customersRepository.Setup(x => x.GetEmailById(_customerId))
                .ReturnsAsync("anamoura@outlook.com");


            var service = new CustomerService(_mockUnitWork.Object, validatorMock.Object);

            _mockUnitWork.Setup(uow => uow.CustomersRepository).Returns(_customersRepository.Object);

            //Act

            var result = await service.UpdateCustomer(newCustomerDTO, _customerId);

            //Assert

            Assert.True(result.IsSuccess);


        }


        //[Test]
        //public void Test1()
        //{
        //    _customersRepository.Setup(x => x.Read(It.IsAny<Expression<Func<Customers, bool>>>()))
        //        .Returns(Task.FromResult(_customers.AsEnumerable()));

        //    _mockUnitWork.Setup(uow => uow.CustomersRepository).Returns(_customersRepository.Object);
        //    var a = _service.GetAll();
        //    Assert.Pass();
        //}

        //[Test]
        //public void Test2()
        //{
        //    _customersRepository.Setup(x => x.GetById(new Guid("A5527163-D9F8-4241-98E9-907BEFCCF117")))
        //        .Returns(Task.FromResult(_customers.AsEnumerable().FirstOrDefault()));

        //    _mockUnitWork.Setup(uow => uow.CustomersRepository).Returns(_customersRepository.Object);
        //    var a = _service.GetById(new Guid("A5527163-D9F8-4241-98E9-907BEFCCF117"));
        //    Assert.Pass();
        //}
    }
}