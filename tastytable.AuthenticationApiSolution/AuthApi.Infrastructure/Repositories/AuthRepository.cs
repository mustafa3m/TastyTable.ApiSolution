using AuthApi.Application.DTOs;
using AuthApi.Application.Interface;
using AuthApi.Domain.Models;
using AuthApi.Infrastructure.Data;
using BCrypt.Net;
using ecommerce.SharedLibrary.response;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AuthApi.Infrastructure.Repositories
{
    public class AuthRepository(AuthDbContext db, IConfiguration config) : IAuth
    {


        private async Task<User> FindUserByEmail(string email)
        {
            User? user = await  db.Users.Where(u => u.Email == email).FirstOrDefaultAsync();
            return user is not null ? user : null!;
        }
        public async Task<User> GetUser(int userId)
        {
            var user = await db.Users.FindAsync(userId);
            return user is not null ? user : null!;
        }

        private string GenerateToken(User user)
        {
            var key = Encoding.UTF8.GetBytes(config.GetSection("Authentication:key").Value!);
            var securityKey = new SymmetricSecurityKey(key);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.Name),
                new(ClaimTypes.Email, user.Email),
                
            };

            if (!string.IsNullOrEmpty(user.Role))
            {
                claims.Add(new (ClaimTypes.Role, user.Role));
            }

            var token = new JwtSecurityToken(
                issuer: config["Authentication:Issuer"],
                audience : config["Authentication:Audience"],
                claims: claims,
                expires : null,
                signingCredentials : credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token); 

        }




        public async Task<Response> Register(RegisterRequestDTO register)
        {
            var user = await FindUserByEmail(register.Email);
            if (user != null)
                return new Response(false, $"{register.Name} Already exits, Please go to login page ");
            var result = db.Users.Add(new User
            {
                Name = register.Name,
                PhoneNumber = register.PhoneNumber,
                Email = register.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(register.Password),
                Role = register.Role,
            }); ;

            await db.SaveChangesAsync();

            return result.Entity.Id > 0 ? new Response(true, "User Registered successfully!") :
               new Response(false, "Invalid data provided");

        }

        public async Task<Response> Login(LoginRequestDTO login)
        {
            var user = await FindUserByEmail(login.Email);
            if (user == null)
                return new Response(false, "Invalid credentials");
            bool verifyPassword = BCrypt.Net.BCrypt.Verify(login.Password, user.Password);
            if (!verifyPassword)
                return new Response(false, "Invalid Email or Password");
            var token = GenerateToken(user);
            return new Response(true, token);
        }
    }
}
