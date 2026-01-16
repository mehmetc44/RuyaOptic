using AutoMapper;
using RuyaOptik.DTO.Category;
using RuyaOptik.DTO.Product;
using RuyaOptik.DTO.Inventory;
using RuyaOptik.DTO.Order;
using RuyaOptik.DTO.Cart;
using RuyaOptik.Entity.Concrete;
using RuyaOptik.DTO.Auth;
using RuyaOptik.Entity.Identity;
namespace RuyaOptik.Business.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Category
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<CategoryCreateDto, Category>();
            CreateMap<CategoryUpdateDto, Category>();

            // Product
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<ProductCreateDto, Product>();
            CreateMap<ProductUpdateDto, Product>();

            // Inventory
            CreateMap<Inventory, InventoryDto>().ReverseMap();
            CreateMap<InventoryCreateDto, Inventory>();
            CreateMap<InventoryUpdateDto, Inventory>();

            // Order
            CreateMap<Order, OrderDto>();

            // Cart
            CreateMap<Cart, CartDto>();
            CreateMap<CartItem, CartItemDto>()
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product.Name))
                .ForMember(d => d.TotalPrice, o => o.MapFrom(s => s.UnitPrice * s.Quantity));
            CreateMap<AspUserRegisterDto, AspUser>();
            CreateMap<AspUser, AspUserRegisterResponseDto>();
        }
    }
}
