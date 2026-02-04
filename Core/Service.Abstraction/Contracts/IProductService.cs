using Shared;
using Shared.Dtos.Enums;
using Shared.Dtos.ProductModule;
using Shared.Parameters;

namespace Service.Abstraction.Contracts
{
    public interface IProductService
    {
        Task<PaginatedResult<ProductResultDto>> GetAllProductsAsync(ProductSpecificationParameters parameters);
        Task<ProductResultDto?> GetProductByIdAsync(int id);
        Task<IEnumerable<BrandResultDto>> GetAllBrandsAsync();
        Task<IEnumerable<TypeResultDto>> GetAllTypesAsync();

    }
}
