using AutoMapper;
using MenuApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuApi.Application.DTOs.Mapper
{
    public class MenuAutoMapper : Profile
    {
        public MenuAutoMapper()
        {

            CreateMap<Menu, MenuDTO>().ReverseMap();    
            
        }
    }
}
