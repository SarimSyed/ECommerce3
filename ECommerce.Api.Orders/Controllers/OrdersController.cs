using ECommerce.Api.Orders.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ECommerce.Api.Orders.Controllers
{
    [ApiController]
    [Route("api/orders")]
    [Produces("application/json")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersProvider ordersProvider;

        public OrdersController(IOrdersProvider ordersProvider)
        {
            this.ordersProvider = ordersProvider;
        }

        /// <summary>
        /// Retrieves all orders associated with customer's id
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns>IActionResult</returns>
        /// <response code="200">Returns the requested customer orders</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetOrdersAsync(int customerId)
        {
            var result = await ordersProvider.GetOrdersAsync(customerId);
            if (result.IsSuccess)
            {
                return Ok(result.Orders);
            }
            return NotFound();
        }
    }
}