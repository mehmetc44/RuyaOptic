using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using RuyaOptik.DTO.AuthDtos;
using RuyaOptik.Entity.Identity;
namespace RuyaOptik.Business.Mapping
{
    public class AutoMappingProfile : Profile
    {
        public AutoMappingProfile()
        {
            CreateMap<AspUserRegisterDto, AspUser>();
            CreateMap<AspUser, AspUserRegisterResponseDto>();
        }
        
    
    }
}