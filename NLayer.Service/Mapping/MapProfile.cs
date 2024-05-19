using AutoMapper;
using NLayer.Core;
using NLayer.Core.DTOs;
using NLayer.Core.Models;

namespace NLayer.Service.Mapping
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<ProductFeature, ProductFeatureDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<CreditCard, CreditCardDto>().ReverseMap();
            CreateMap<Address, AddressDto>().ReverseMap();
            CreateMap<ProductUpdateDto, Product>();
            CreateMap<AddressUpdateDto, Address>();
            CreateMap<Product, ProductWithCategoryDto>();
            CreateMap<Category, CategoryWithProductsDto>();
            CreateMap<User, UsersWithCreditCardsAndAddressesDto>();
            CreateMap<Address, AddressesWithUsersDto>();
            CreateMap<CreditCard, CreditCardsWithUsersDto>();
        }
    }
}
