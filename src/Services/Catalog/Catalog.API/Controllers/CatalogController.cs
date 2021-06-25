using Catalog.API.Entities;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Catalog.API.DTO;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<CatalogController> _logger;
        private readonly IMapper _mapper;

        public CatalogController(IProductRepository productRepository, ILogger<CatalogController> logger, IMapper mapper)
        {
            _productRepository = productRepository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
        {
            var products = (await _productRepository.GetProducts()).ToList();
            return _mapper.Map<List<ProductDTO>>(products);
        }

        [HttpGet("{id:length(24)}", Name = nameof(GetProductById))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ProductDTO>> GetProductById(string id)
        {
            var product = await _productRepository.GetProduct(id);
            if (product == null)
            {
                _logger.LogError($"Product with id: {id}, not found.");
                return NotFound();
            }
            return _mapper.Map<ProductDTO>(product);
        }

        [HttpGet("[action]/{category}", Name = nameof(GetProductsByCategory))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductsByCategory(string category)
        {
            var products = (await _productRepository.GetProductsByCategory(category)).ToList();
            return _mapper.Map<List<ProductDTO>>(products);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ProductDTO>> CreateProduct([FromBody] ProductDTO productDto)
        {
            await _productRepository.CreateProduct(_mapper.Map<Product>(productDto));
            return CreatedAtRoute(nameof(GetProductById), new { id = productDto.Id }, productDto);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> UpdateProduct([FromBody] ProductDTO productDto)
        {
            return await _productRepository.UpdateProduct(_mapper.Map<Product>(productDto));
        }

        [HttpDelete("{id:length(24)}", Name = nameof(DeleteProductById))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> DeleteProductById(string id)
        {
            return await _productRepository.DeleteProduct(id);
        }
    }
}
