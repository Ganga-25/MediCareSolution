using MediCare.Application.DTOs.CredentialDTO;
using MediCare.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MediCare.Application.DTOs.ProfileUpdateDTO
{
    public class DoctorUpdateDTO
    {
        public int? DepartmentId { get; set; }
        public string? ProfilePhoto { get; set; }
        public string? ContactNumber { get; set; }
        public string? MedicalRegistrationNumber { get; set; } 
        public Decimal? Fees { get; set; }
        public int? Experience { get; set; }
    }
    public class DoctorDTO
    {
        public int DoctorId { get; set; }
        public int UserId { get; set; }
        public int? DepartmentId { get; set; }
        public string? ProfilePhoto { get; set; }
        public string? ContactNumber { get; set; }
        public string? MedicalRegistrationNumber { get; set; } 
        public Decimal? Fees { get; set; }
        public int? Experience { get; set; }
        public Veri_Status VerificationStatus { get; set; } 
        public bool IsAvailable { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<DoctorCredentialDTO>? Credentials { get; set; }
    }

    public class DoctorVerificationUpdateDTO
    {
        public int DoctorId { get; set; }
        public Veri_Status VerificationStatus { get; set; } // "Verified" or "Rejected"
        public int ModifiedBy { get; set; } // set from JWT in service/controller
    }
}
