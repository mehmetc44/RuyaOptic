using AutoMapper;
using RuyaOptik.DTO.Category;
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
        }
    }
}
