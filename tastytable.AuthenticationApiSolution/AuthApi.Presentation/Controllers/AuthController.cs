using AuthApi.Application.DTOs;
using AuthApi.Application.Interface;
using AuthApi.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthApi.Presentation.Controllers
{
    [Route("api/auth")]
    [ApiController]
    
    public class AuthController(IAuth authInterface) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterRequestDTO userModel)
        {
            if(!ModelState.IsValid) 
                return BadRequest(ModelState);
            var response = await authInterface.Register(userModel);
            return response.Flag ? Ok(response) : BadRequest(response);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginRequestDTO login)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await authInterface.Login(login);
            return response.Flag ? Ok(response) : BadRequest(response); 

        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id:int}")]

        public async Task<ActionResult<User>> GetUser(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid user id");
            var user = await authInterface.GetUser(id);
            return user is not null ? Ok(user) : BadRequest("Error occurred while retrieving user");
        }

    }
}
