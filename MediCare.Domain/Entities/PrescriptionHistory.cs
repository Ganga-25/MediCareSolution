using MediCare.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Domain.Entities
{
    public class PrescriptionHistory
    {
        public int HistoryId { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public int PrescriptionId { get; set; }
        public Actions Actions { get; set; } 
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
