
using E_Commerce.Factories;
using Microsoft.AspNetCore.Http;
using Presentation.Attributes;
using Shared;
using Shared.Dtos;
using Shared.Dtos.Enums;
using Shared.Dtos.ProductModule;
using Shared.ErrorModels;
using Shared.Parameters;

namespace Presentation.Controllers
{
    
    public class ProductsController(IServiceManager _serviceManager) : ApiController
    {
        [RedisCache]
        // Get all products
        [HttpGet]
        public async Task<ActionResult<PaginatedResult<ProductResultDto>>> GetAllProducts([FromQuery]ProductSpecificationParameters parameters)
            => Ok(await _serviceManager.ProductService.GetAllProductsAsync(parameters));

        //Get all Brands
        [HttpGet("Brands")] // BaseUrl/Products/Brands  
        public async Task<ActionResult<IEnumerable<BrandResultDto>>> GetAllBrands()
            => Ok(await _serviceManager.ProductService.GetAllBrandsAsync());

        //Get all Types
        [HttpGet("Types")]  
        public async Task<ActionResult<IEnumerable<TypeResultDto>>> GetAllTypes()
            => Ok(await _serviceManager.ProductService.GetAllTypesAsync());

        //Get Product By Id
        [ProducesResponseType(typeof(ProductResultDto),StatusCodes.Status200OK)]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductResultDto>> GetProductById(int id)
            => Ok(await _serviceManager.ProductService.GetProductByIdAsync(id));
    }
}
