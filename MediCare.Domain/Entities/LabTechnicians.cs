using MediCare.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Domain.Entities
{
    public class LabTechnicians:BaseEntity
    {
        public int LabTechnicianId { get; set; }
        public int UserId { get; set; }
        public int DepartmentId { get; set; }
        public string LicenceNumber { get; set; } = null!;
        public string ContactNumber { get; set; } = string.Empty;

        public string ProfilePhoto { get; set; }= string.Empty;
        public Veri_Status VerificationStatus { get; set; } = Veri_Status.Pending;
        public bool IsAvailable { get; set; }=false;

    }
}
