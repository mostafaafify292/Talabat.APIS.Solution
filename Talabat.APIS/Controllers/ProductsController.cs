using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Data;
using Talabat.APIS.DTOs;
using Talabat.APIS.Errors;
using Talabat.APIS.Helpers;
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
        private readonly IGenericRepository<ProductBrand> _brandRepo;
        private readonly IGenericRepository<ProductCategory> _categoryRepo;

        public ProductsController(IGenericRepository<Product> productRepo ,
                                  IMapper mapper ,
                                  IGenericRepository<ProductBrand> brandRepo,
                                  IGenericRepository<ProductCategory> categoryRepo)
        {
            _productRepo = productRepo;
            _mapper = mapper;
            _brandRepo = brandRepo;
            _categoryRepo = categoryRepo;
        }



        [HttpGet]
        [CachedAttribute(300)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme )]
        public async Task<ActionResult<IReadOnlyList<ProductDTO>>> GetAllProduct([FromQuery]ProductSpecParams specParams)
        {
            var spec = new ProductWithBrandAndCategorySpecification(specParams);
            var products = await _productRepo.GetAllWithSpecAsync(spec);

            var Data = _mapper.Map<IReadOnlyList<Product>,IReadOnlyList<ProductDTO>>(products);
            var specForCount = new ProductWithBrandAndCategorySpecification(specParams.brandId,specParams.categoryId,specParams.SearchByName);
            var Count =await _productRepo.GetCountForPaginaion(specForCount);
            return Ok(new Pagination<ProductDTO>(specParams.pagesize ,specParams.pageIndex,Count ,Data));

        }





        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductDTO) , StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse) , StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            var spec = new ProductWithBrandAndCategorySpecification(id);
            var product = await _productRepo.GetWithSpecAsync(spec);
            if (product == null)
            {
                
                return NotFound(new ApiResponse(404));
            }
            var mappedProduct = _mapper.Map<Product,ProductDTO>(product);
            return Ok(mappedProduct);    
        }




        [HttpGet("brands")]
        [CachedAttribute(300)]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetAllBrand()
        {
            var brands =await _brandRepo.GetAllAsync();
            return Ok(brands); 
        }




        [HttpGet("categories")]
        [CachedAttribute(300)]
        public async Task<ActionResult<IReadOnlyList<ProductCategory>>> GetAllCategories()
        {
            var categories = await _categoryRepo.GetAllAsync();
            return Ok(categories);
        }
    }
}
