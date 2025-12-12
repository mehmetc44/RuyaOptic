using AutoMapper;
using RuyaOptik.DTO.Category;
using RuyaOptik.DTO.Product;
using RuyaOptik.DTO.Inventory;
using RuyaOptik.Entity.Entities.Concrete;

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
        }
    }
}
