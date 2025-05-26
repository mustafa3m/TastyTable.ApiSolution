using AuthApi.Application.DTOs;
using AuthApi.Domain.Models;
using ecommerce.SharedLibrary.response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthApi.Application.Interface
{
    public interface IAuth
    {
        Task<Response> Register(RegisterRequestDTO register);
        Task<Response> Login(LoginRequestDTO login);
        Task<User> GetUser(int userId);
    }
}
