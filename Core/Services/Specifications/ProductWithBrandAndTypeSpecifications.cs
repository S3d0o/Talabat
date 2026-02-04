using Domain.Entities.ProductModule;
using Shared.Dtos.Enums;
using Shared.Parameters;

namespace Services.Specifications
{
    internal class ProductWithBrandAndTypeSpecifications : BaseSpecification<Product,int>
    {
        public ProductWithBrandAndTypeSpecifications(ProductSpecificationParameters parameters)
            : base(p => (!parameters.TypeId.HasValue || p.TypeId == parameters.TypeId) &&
                                (!parameters.BrandId.HasValue || p.BrandId == parameters.BrandId)&&
                                (string.IsNullOrEmpty(parameters.Search) || p.Name.ToLower().Contains(parameters.Search.ToLower()))                                                                 )
        {
            AddInclude(p => p.ProductType);
            AddInclude(p => p.ProductBrand);
            if (parameters.sort.HasValue)
            {
                switch (parameters.sort)
                {
                    case ProductSortingOptions.NameAsc:
                        AddOrderBy(p => p.Name);
                        break;
                    case ProductSortingOptions.NameDesc:
                        AddOrderByDescending(p => p.Name);
                        break;
                    case ProductSortingOptions.PriceAsc:
                        AddOrderBy(p => p.Price);
                        break;
                    case ProductSortingOptions.PriceDesc:
                        AddOrderByDescending(p => p.Price);
                        break;
                    default:
                        AddOrderBy(p => p.id);
                        break;
                }
            }
            
           // Pagination
            ApplyPagination(parameters.PageSize, parameters.PageIndex);
        }
        // Get product by id overload 
        public ProductWithBrandAndTypeSpecifications(int id) : base(p=>p.id == id)
        {
            AddInclude(p => p.ProductType);
            AddInclude(p => p.ProductBrand);
        }
    }
}
