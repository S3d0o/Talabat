using Domain.Entities.IdentityModule;
using Domain.Entities.OrderModule;
using Shared.Dtos.OrderModule;

namespace Services.MappingProfiles
{
    internal class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<OrderAddress, AddressDto>().ReverseMap();
            CreateMap<Address, AddressDto>().ReverseMap();
            CreateMap<DeliveryMethod, DeliveryMethodResult>()
                .ForMember(dest=>dest.Cost,opt=>opt.MapFrom(src=>src.Price));
            CreateMap<OrderItem,OrderItemDto>()
                .ForMember(dest=>dest.ProductId,opt=>opt.MapFrom(src=>src.ProductItemOrdered.ProductId))
                .ForMember(dest=>dest.ProductName,opt=>opt.MapFrom(src=>src.ProductItemOrdered.ProductName))
                .ForMember(dest=>dest.PictureUrl,opt=>opt.MapFrom(src=>src.ProductItemOrdered.PictureUrl));
            CreateMap<Order, OrderResult>()
                .ForMember(dest => dest.PaymentStatus, opt => opt.MapFrom(src => src.PaymentStatus.ToString()))
                .ForMember(dest => dest.DeliveryMethod, opt => opt.MapFrom(src => src.DeliveryMethod.ShortName))
                .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.SubTotal + (src.DeliveryMethod != null ? src.DeliveryMethod.Price : 0)));
        }
    }
}
