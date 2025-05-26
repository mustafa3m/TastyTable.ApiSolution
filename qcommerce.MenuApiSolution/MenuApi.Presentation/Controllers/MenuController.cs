using ecommerce.SharedLibrary.response;
using MenuApi.Application.DTOs;
using MenuApi.Application.DTOs.Mapper;
using MenuApi.Application.Interface;
using MenuApi.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MenuApi.Presentation.Controllers
{
    [Route("api/menu")]
    [ApiController]
  
    public class MenuController(IMenu menuInterface) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<MenuDTO>> GetAllMenu()
        {
            IEnumerable<Menu> listOfMenu = await menuInterface.GetAllAsync();
            if(!listOfMenu.Any())
                return NotFound("No menus detected in the database");
            var(noData, list) = MenuMapper.FromEntity(null, listOfMenu);
            return list is not null ? Ok(list) : NotFound("No menus found");
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult<MenuDTO>> GetById(int id)
        {
            var menu = await menuInterface.FindByIdAsync(id);
            if (menu == null)
                return NotFound("No menus found");
            var (_menu, noData) = MenuMapper.FromEntity(menu, null);
            return Ok(_menu);
        }

        [HttpPost]
        public async Task<ActionResult<Response>> CreateMenu(MenuDTO menuDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data");

            var addMenu = MenuMapper.ToEntity(menuDTO, null);

            var response = await menuInterface.CreateAsync(addMenu);
            return response.Flag ? Ok(response) : BadRequest(response);
        }


        [HttpPut]
        public async Task<ActionResult<Response>> UpdateMenu(MenuDTO menuDTO)
        {
            var menu = MenuMapper.ToEntity(menuDTO, null!);
            var response = await menuInterface.UpdateAsync(menu);
            return response.Flag ? Ok(response) : BadRequest(response);
        }


        [HttpDelete]

        public async Task<ActionResult<Response>> DeleteMenu(MenuDTO menuDTO)
        {
            Menu menuToDelete = MenuMapper.ToEntity(menuDTO, null!);

            var response = await menuInterface.DeleteAsync(menuToDelete);

            return response.Flag ? Ok(response) : NotFound(response);
        }

       



    }
}
