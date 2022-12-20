using ECommerce.Api.Search.Interfaces;
using ECommerce.Api.Search.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ECommerce.Api.Search.Controllers
{
    [ApiController]
    [Route("api/search")]
    [Produces("aaplication/json")]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService searchService;

        public SearchController(ISearchService searchService)
        {
            this.searchService = searchService;
        }
        /// <summary>
        /// Returns order by search term
        /// </summary>
        /// <param name="term"></param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SearchAsync(SearchTerm term)
        {
            var result = await searchService.SearchAsync(term.CustomerId);
            if (result.IsSuccess)
            {
                return Ok(result.SearchResults);
            }
            return NotFound();
        }
    }
}