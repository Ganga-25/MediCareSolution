using MediCare.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Application.DTOs.UsersDTO
{
    public class DoctorInformationDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        
        public int DoctorId { get; set; }
       
        public int? DepartmentId { get; set; }
        public string? ProfilePhoto { get; set; }
        public string? ContactNumber { get; set; }
        public string? MedicalRegistrationNumber { get; set; } 
        public Decimal? Fees { get; set; }
        public int? Experience { get; set; }
        public Veri_Status VerificationStatus { get; set; } 
        public bool IsAvailable { get; set; } 
    }
}
