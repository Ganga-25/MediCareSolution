using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Application.DTOs.AuthDTO
{
    public class LoginRequestDTO
    {
        public string UserEmail { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
