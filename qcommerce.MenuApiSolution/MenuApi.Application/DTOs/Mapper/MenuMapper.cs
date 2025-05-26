using MenuApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace MenuApi.Application.DTOs.Mapper
{
    public static class MenuMapper 
    {
        public static Menu ToEntity(MenuDTO menuDTO, MenuUpdateDTO updateDTO)
        {

            Menu menu = new Menu()
            {
                Id = menuDTO.Id,
                Name = menuDTO.Name,
                Price = menuDTO.Price,
            
            };
            return menu;
   
        }


        public static(MenuDTO?, IEnumerable<MenuDTO>?) FromEntity(Menu menu, IEnumerable<Menu> listOfMenu)
        {
            if(menu is not null)
            {
                MenuDTO singleMenu = new()
                {
                    Id = menu.Id,
                    Name = menu.Name,
                    Price = menu.Price,
              
                };
                return (singleMenu, null);
            }

            if(listOfMenu is not null)
            {
                 IEnumerable<MenuDTO> menus = listOfMenu.Select(p => new MenuDTO()
                {
                     Id = p.Id,
                     Name = p.Name,
                     Price = p.Price,
                  
                 });

                return(null, menus);
            }

            return (null, null);
        }
    }
}
