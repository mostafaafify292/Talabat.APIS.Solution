using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIS.DTOs;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Specifications;

namespace Talabat.APIS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IMapper _mapper;

        public ProductsController(IGenericRepository<Product> productRepo ,IMapper mapper)
        {
            _productRepo = productRepo;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAllProduct()
        {
            var spec = new ProductWithBrandAndCategorySpecification();
            var products = await _productRepo.GetAllWithSpecAsync(spec);
            var mappedProduct = _mapper.Map<IEnumerable<Product>,IEnumerable<ProductDTO>>(products);
            return Ok(mappedProduct);

        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            var spec = new ProductWithBrandAndCategorySpecification(id);
            var product = await _productRepo.GetWithSpecAsync(spec);
            if (product == null)
            {
                
                return NotFound();
            }
            var mappedProduct = _mapper.Map<Product,ProductDTO>(product);
            return Ok(mappedProduct);
           
            
        }
    }
}
