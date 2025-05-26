using ecommerce.SharedLibrary.response;
using FakeItEasy;
using FluentAssertions;
using MenuApi.Application.DTOs;
using MenuApi.Application.Interface;
using MenuApi.Domain.Entities;
using MenuApi.Presentation.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.MenuApi
{
    public class MenuControllerTest
    {
        private readonly  IMenu menuInterface;
        private readonly MenuController menuController;


        public MenuControllerTest()
        {
            menuInterface = A.Fake<IMenu>();
            menuController = new MenuController(menuInterface);
        }



        [Fact]
        public async Task GetMenu_WhenMenuExists_ReturnOkResponseWithMenu()
        {
            var menus = new List<Menu>()
            {
                new Menu() {Id = 1, Name = "Test1", Price = 10},
                new Menu() {Id = 2, Name = "Test2", Price = 5}
            };

            A.CallTo(() => menuInterface.GetAllAsync()).Returns(menus);


            var result = await menuController.GetAllMenu();

            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(StatusCodes.Status200OK);


            var returnedMenu = okResult.Value as IEnumerable<MenuDTO>;

            returnedMenu.Should().NotBeNull();
            returnedMenu.Should().HaveCount(2);
            returnedMenu.First().Id.Should().Be(1);   
            returnedMenu.Last().Id.Should().Be(2);
        }



        [Fact]
        public async Task GetAllMenu_WhenNoMenusExists_ReturnNotFoundResponse()
        {
            var menus = new List<Menu>();

            A.CallTo(() => menuInterface.GetAllAsync()).Returns(menus);

            var result = await menuController.GetAllMenu();

            var noFoundResult = result.Result as NotFoundObjectResult;
            noFoundResult.Should().NotBeNull();
            noFoundResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);

            var message = noFoundResult.Value as string;
            message.Should().Be("No menus detected in the database");

        }



        [Fact]
        public async Task CreateMenu_WhenModelIsInvalid_ReturnBadRequest()
        {
            var menuDto = new MenuDTO()
            {
                Id = 1,
                Price = 120,
            };


            menuController.ModelState.AddModelError("Name", "Required");

            var result = await menuController.CreateMenu(menuDto);

            var badRequestResult = result.Result as BadRequestObjectResult;

            badRequestResult.Should().NotBeNull();
            badRequestResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            badRequestResult.Value.Should().Be("Invalid data");
        }




        [Fact]
        public async Task CreateMenu_WhenCreateIsSuccessfull_ReturnOkResponse()
        {
            var menuDto = new MenuDTO();
            var response = new Response(true, "Created");

            A.CallTo(() => menuInterface.CreateAsync(A<Menu>.Ignored)).Returns(response);
        }







       
    }
}
