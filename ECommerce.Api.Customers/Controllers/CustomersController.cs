using ECommerce.Api.Customers.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ECommerce.Api.Customers.Controllers
{
    [ApiController]
    [Route("api/customers")]
    [Produces("application/json")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomersProvider customersProvider;

        public CustomersController(ICustomersProvider customersProvider)
        {
            this.customersProvider = customersProvider;
        }

        /// <summary>
        /// Returns all customers
        /// </summary>
        /// <returns>IActionResult</returns>
        /// <response code="200">Returns requeested customer</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        public async Task<IActionResult> GetCustomersAsync()
        {
            var result = await customersProvider.GetCustomersAsync();
            if (result.IsSuccess)
            {
                return Ok(result.Customers);
            }
            return NotFound();
        }
        /// <summary>
        /// Get customer by provided id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>IAction result</returns>
        /// <response code="200">Returns requeested customer</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCustomerAsync(int id)
        {
            var result = await customersProvider.GetCustomerAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result.Customer);
            }
            return NotFound();
        }
    }
}