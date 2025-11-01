using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Application.DTOs.PrescriptionDTO
{
    public class AddPresciptionDTO
    {
        public int AppointmentId { get; set; }
        public DateOnly PrescriptionDate { get; set; }
        public string? Summary { get; set; }

        // Medication Details
        public string Medicine { get; set; } = null!;
        public string Dosage { get; set; } = null!;
        public int NoOfDays { get; set; }
        public string? Instructions { get; set; }
    }
}
