using MediCare.Application.DTOs.LabTechnicianDTO;
using MediCare.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
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
        [JsonIgnore(Condition =JsonIgnoreCondition.WhenWritingNull)]
        public List<LabTechnicianCredentialDTO>? Credentials { get; set; }
    }
    public class LabTechnicianUpdateDTO
    {
        
        public int? DepartmentId { get; set; }
        public string? LicenceNumber { get; set; }
        public string? ContactNumber { get; set; }

        public string? ProfilePhoto { get; set; }
        
    }
    public class UpdateverificationstatusofLabtechDTO
    {
        public int LabTechnicianId { get; set; }
        public Veri_Status VerificationStatus { get; set; }
     
    }
}
