using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        [ProducesResponseType(typeof(Task<IEnumerable<Product>>), StatusCodes.Status200OK)]
        public ActionResult<Task<IEnumerable<Product>>> GetProducts()
        {
            return _repository.GetProducts();
        }

        [HttpGet(template: "{id:length(24)}", Name = "GetProduct")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        public async Task<ActionResult<Product>> GetProductById([FromRoute] string id)
        {
            var product = await _repository.GetProduct(id);
            if (product != null) 
                return product;
            _logger.LogWarning($"Product with id: {id} not found");
            return NotFound();

        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status200OK)]
        public ActionResult<Task<IEnumerable<Product>>> GetProductsByCategory(string category)
        {
            return _repository.GetProductByCategory(category);
        }

        [HttpPost("[action]")]
        [ProducesResponseType(typeof(Product), StatusCodes.Status201Created)]
        public async Task<ActionResult<Product>> Create([FromBody] Product product)
        {
            await _repository.Create(product);
            return CreatedAtRoute("GetProduct", new {id = product.Id}, product);
        }


        [HttpPut("[action]")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

        [HttpDelete("[action]/{id:length(24)}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> Delete(string id)
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
