using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Domain.Entities
{
    public class Prescription:BaseEntity
    {
        public int PrescriptionId { get; set; }

        // Core Prescription Info
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateOnly PrescriptionDate { get; set; }
        public string? Summary { get; set; }

        // Medication Details
        public string Medicine { get; set; } = null!;
        public string Dosage { get; set; } = null!;
        public int NoOfDays { get; set; }
        public string? Instructions { get; set; }

    }
}
