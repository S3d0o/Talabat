using AutoMapper;
using Domain.Entities.ProductModule;
using Shared.Dtos.ProductModule;
using System.Runtime.InteropServices;

namespace Services.MappingProfiles
{
    internal class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductResultDto>().
                ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.ProductBrand.Name))
                .ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => src.ProductType.Name)) // needs loading related data
                .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom<PictureUrlResolver>());

            CreateMap<ProductBrand, BrandResultDto>();
            CreateMap<ProductType, TypeResultDto>();

        }
    }
}
