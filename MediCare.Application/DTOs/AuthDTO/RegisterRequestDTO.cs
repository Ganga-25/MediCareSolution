using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Application.DTOs
{
    public class RegisterRequestDTO
    {
        public string UserName { get; set; } = null!;
        public string UserEmail { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
