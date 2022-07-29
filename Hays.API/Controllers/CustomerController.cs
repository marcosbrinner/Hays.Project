using Hays.Domain.Entities;
using Hays.Domain.Abstraction.Services;
using Hays.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Hays.Domain.DTO;

namespace Hays.API.Controllers
{
    [Route("api/customers")]
    [ApiController]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class CustomersController : MainController
    {
        private readonly ICustomerService _customerService;
        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerViewDTO>>> Get()
        {
            var result = await _customerService.GetAll();

            return Ok(result);
        }

        [HttpGet("{customerId:guid}")]
        public async Task<ActionResult<CustomerViewDTO?>> GetById(Guid customerId)
        {
            var result = await _customerService.GetById(customerId);
            if (result == null)
                return NotFound();
            else
                return Ok(result);
        }

        [HttpPost("create")]
        public async Task<ActionResult<Bag<Customers>>> Create(Domain.DTO.CustomerDTO data)
        {
            var result = await _customerService.CretaeNewCustomer(data);
            return Result(result);
        }

        [HttpPut("update/{customerId:guid}")]
        public async Task<ActionResult<Bag<Customers>>> Update(Domain.DTO.CustomerDTO data, Guid customerId)
        {
            var result = await _customerService.UpdateCustomer(data, customerId);
            return Result<Customers>(result);
        }

    }

}
