using MediCare.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Application.DTOs.ProfileUpdateDTO
{
    public class LabTechnicianDTO
    {
        public int LabTechnicianId { get; set; }
        public int UserId { get; set; }
        public int? DepartmentId { get; set; }
        public string? LicenceNumber { get; set; }
        public string? ContactNumber { get; set; }
        public string? ProfilePhoto { get; set; }
        public string? VerificationStatus { get; set; }
        public bool IsActive { get; set; }
    }
    public class LabTechnicianUpdateDTO
    {
        
        public int? DepartmentId { get; set; }
        public string? LicenceNumber { get; set; }
        public string? ContactNumber { get; set; }

        public string? ProfilePhoto { get; set; }
        
    }
}
