using MediCare.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Domain.Entities
{
    public class Doctors:BaseEntity
    {
        public int  DoctorId {  get; set; }
        public int UserId { get; set; }
        public int? DepartmentId { get; set; }
        public string? ProfilePhoto { get; set; } 
        public string? ContactNumber { get; set; }
        public string? MedicalRegistrationNumber { get; set; } = null!;
        public Decimal? Fees { get; set; }
        public int? Experience { get; set; }
        public Veri_Status VerificationStatus { get; set; } = Veri_Status.Pending;
        public bool IsAvailable { get; set; } = false;

    }
}