using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Application.DTOs.UsersDTO
{
    public class PatientInformationDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; } 
        public string UserEmail { get; set; } 
        public string HashPassword { get; set; }
        public int PatientId { get; set; }
        public string UHID { get; set; } 
        
        public int? Age { get; set; }
        public string? ContactNumber { get; set; }
        public string? Gender { get; set; }
    }
}
