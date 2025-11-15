using MediCare.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Application.DTOs.UsersDTO
{
    public class LabtechnicianInformationDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string HashPassword { get; set; }
        public int LabTechnicianId { get; set; }
      
        public int? DepartmentId { get; set; }
        public string? LicenceNumber { get; set; }
        public string? ContactNumber { get; set; }

        public string? ProfilePhoto { get; set; }
        public Veri_Status VerificationStatus { get; set; } 
        public bool IsActive { get; set; } 
    }
}
