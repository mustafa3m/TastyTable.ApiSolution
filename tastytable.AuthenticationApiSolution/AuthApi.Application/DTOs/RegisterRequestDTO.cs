using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthApi.Application.DTOs
{
    public class RegisterRequestDTO
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        //public string Address { get; set; }
        //public string City { get; set; }
        public string Email { get; set; } = "Maslah@hotmail.com";
        public string Password { get; set; }
        public string Role { get; set; }
        public DateTime RegisteredDate { get; set; } = DateTime.Now;
    }
}
