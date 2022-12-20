using ECommerce.Api.Products.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ECommerce.Api.Products.Controllers
{
    [ApiController]
    [Route("api/products")]
    [Produces("application/json")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsProvider productsProvider;

        public ProductsController(IProductsProvider productsProvider)
        {
            this.productsProvider = productsProvider;
        }
        /// <summary>
        /// Get all products
        /// </summary>
        /// <returns>IActionResult</returns>
        /// <response code="200">Returns requested product</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProductsAsync()
        {
            var all = await productsProvider.GetProductsAsync();
            if (all.IsSuccess)
            {
                return Ok(all.Products);
            }
            return NotFound(all.ErrorMessage);
        }
        /// <summary>
        /// Get product by provided id
        /// </summary>
        /// <param name="id">The product id to get by</param>
        /// <returns>IActionResult</returns>
        /// <response code="200">Returns requested product</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProductAsync(int id)
        {
            var result = await productsProvider.GetProductAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result.Product);
            }
            return NotFound(result.ErrorMessage);
        }
    }
}