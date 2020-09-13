using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Catalog.API.Entities;
using Catalog.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly IProductRepository _repository;
        private readonly ILogger<CatalogController> _logger;
        public CatalogController(
            IProductRepository repository, 
            ILogger<CatalogController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return Ok(await _repository.GetProducts());
        }

        [HttpGet(template: "{id}", Name = "GetProduct")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<ActionResult<Product>> GetProductById([StringLength(24), FromRoute] string id)
        {
            var product = await _repository.GetProduct(id);
            if (product != null) 
                return product;
            _logger.LogWarning($"Product with id: {id} not found");
            return NotFound();

        }

        [HttpGet("{category}")]
        [ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByCategory(string category)
        {
            return Ok(await _repository.GetProductByCategory(category));
        }

        [HttpPost("[action]")]
        [ProducesResponseType(typeof(Product), StatusCodes.Status201Created)]
        [Produces("application/json")]
        public async Task<ActionResult<Product>> Create([FromBody] Product product)
        {
            await _repository.Create(product);
            return CreatedAtRoute("GetProduct", new {id = product.Id}, product);
        }


        [HttpPut("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public async Task<ActionResult<bool>> Update([FromBody] Product product)
        {
            var result = await _repository.Update(product);
            if (result)
                return true;
            return new ObjectResult(false)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }


        /// <summary>
        /// Delete Catalog
        /// </summary>
        /// <remarks>
        /// Return bool
        /// </remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">OK</response>
        /// <response code="500">Internal server error</response>
        [HttpDelete("[action]/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public async Task<ActionResult<bool>> Delete([StringLength(24), FromRoute] string id)
        {
            var result = await _repository.Delete(id);
            if (result)
                return true;
            return new ObjectResult(false)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };

        }
    }
}
