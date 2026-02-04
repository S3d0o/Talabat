using Domain.Contracts.StoreDb;
using Domain.Exceptions;
using Shared;
using Shared.Dtos.Enums;
using Shared.Dtos.ProductModule;
using Shared.Parameters;

namespace Services.Implementations
{
    public class ProductService(IUnitOfWork _unitOfWork, IMapper _mapper) : IProductService
    {
        public async Task<IEnumerable<BrandResultDto>> GetAllBrandsAsync()
        {
            var brands = await _unitOfWork.GenericRepository<ProductBrand, int>().GetAllAsync();
            return _mapper.Map<IEnumerable<BrandResultDto>>(brands);
        }

        public async Task<PaginatedResult <ProductResultDto>> GetAllProductsAsync(ProductSpecificationParameters parameters)
        {

            var specifications = new ProductWithBrandAndTypeSpecifications(parameters);
            var products = await _unitOfWork.GenericRepository<Product, int>().GetAllAsync(specifications);
             var productResult =_mapper.Map<IEnumerable<ProductResultDto>>(products);
            var pagecount = productResult.Count();
            var countSpecifications = new ProductCountSpecification(parameters);
            var totalCount = await _unitOfWork.GenericRepository<Product, int>().CountAsync(countSpecifications);
            return new PaginatedResult<ProductResultDto>(parameters.PageIndex, pagecount,totalCount , productResult);
           
        }

        public async Task<IEnumerable<TypeResultDto>> GetAllTypesAsync()
        {
            var types = await _unitOfWork.GenericRepository<ProductType, int>().GetAllAsync();
            return _mapper.Map<IEnumerable<TypeResultDto>>(types);
        }

        public async Task<ProductResultDto?> GetProductByIdAsync(int id)
        {
            var specifications = new ProductWithBrandAndTypeSpecifications(id);

            var product = await _unitOfWork.GenericRepository<Product, int>().GetByIdAsync(specifications);
            
            return product is null ? throw new ProductNotFoundException(id) : _mapper.Map<ProductResultDto>(product);
        }
    }
}
