using FluentValidation;
using Hays.Domain.DTO;
using Hays.Domain.Entities;
using Hays.Domain.Abstraction.DataAcess;
using Hays.Domain.Abstraction.Services;
using Hays.Domain.Models;

namespace Hays.Domain.Services
{
    public class CustomerService : ServiceBase<Customers>, ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<Customers> _validator;
        public CustomerService(
            IUnitOfWork unitOfWork,
            IValidator<Customers> validator
            )
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<Bag<Customers>> CretaeNewCustomer(CustomerDTO newCustomer)
        {
            var validations = await ValidateData(newCustomer);

            if (!validations.IsSuccess)
                return validations;

            if (validations.Data == null)
                return ErroResult<Customers>(Const.DataLost);

            validations.Data.Password = BCrypt.Net.BCrypt.HashPassword(validations.Data.Password);

            var result = await RunSafe(() => _unitOfWork.CustomersRepository.Create(validations.Data), validations.Data);


            return result;
        }

        public async Task<Bag<Customers>> UpdateCustomer(CustomerDTO newCustomer, Guid customerId)
        {
            var validations = await ValidateData(newCustomer, true, customerId);

            if (!validations.IsSuccess)
                return validations;

            var customer = await GetByIdToEdit(customerId);
            if (customer == null || validations.Data == null)
                return  ErroResult<Customers>(Const.CustomerNotExists);

            customer.Name = validations.Data.Name;
            customer.Surname = validations.Data.Surname;
            customer.Email = validations.Data.Email.Trim();
            customer.Password = BCrypt.Net.BCrypt.HashPassword(validations.Data.Password);

            var result = await RunSafe(() => _unitOfWork.CustomersRepository.Update(customer), customer);

            return result;
        }

        public async Task<IEnumerable<CustomerViewDTO>> GetAll()
        {
            return await _unitOfWork.CustomersRepository.GetAll();
        }

        public async Task<CustomerViewDTO?> GetById(Guid id)
        {
            return await _unitOfWork.CustomersRepository.GetById(id);
        }

        private async Task<Customers?> GetByIdToEdit(Guid id)
        {
            return await _unitOfWork.CustomersRepository.GetByIdToEdit(id);
        }

        private async Task<Bag<CustomerViewDTO>> CheckIfCustomerExists(Guid customerId)
        {
            var customer = await GetById(customerId);
            if (customer == null)
                return ErroResult<CustomerViewDTO>(Const.CustomerNotExists);
            else
                return OkResult<CustomerViewDTO>(customer);
        }

        private async Task<bool> IsEmailAlreadyRegistrated(CustomerDTO newCustomer)
        {
            return await _unitOfWork.CustomersRepository.CehckIfExistsByEmail(newCustomer.Email);
        }

        private async Task<string> GetEmailById(Guid customerId)
        {
            return await _unitOfWork.CustomersRepository.GetEmailById(customerId) ?? "";
        }

        private async Task<Bag<Customers>> ValidateData(CustomerDTO data, bool update = false, Guid? customerId = null)
        {

            var mapped = Map(data);
            var emailexists = await IsEmailAlreadyRegistrated(data);

            var validation = _validator.Validate(mapped);

            if (!validation.IsValid)
                return ErroResult(validation.Errors, mapped);

            if (update && customerId.HasValue)
            {
                var customerData = await CheckIfCustomerExists(customerId.Value);

                if (!customerData.IsSuccess)
                    return new Bag<Customers>(customerData.Errors.ToList());
            }

            if (emailexists && !update)
                return ErroResult(Const.EmailAlreadyExists, mapped);
            else if ((emailexists && update) && await GetEmailById(customerId.Value) != (data.Email ?? string.Empty))
                return ErroResult(Const.EmailAlreadyExists, mapped);

            return OkResult(mapped);
        }

        private Customers Map(CustomerDTO dTO)
        {
            return new Customers
            {
                Name = dTO.Name,
                Email = dTO.Email?.Trim() ?? "",
                Surname = dTO.Surname,
                Password = dTO.Password,
            };
        }
    }
}
