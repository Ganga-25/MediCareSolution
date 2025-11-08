using MediCare.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Domain.Entities
{
    public class LabTestBooking:BaseEntity
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public int LabTestId { get; set; }
        public int DepartmentId { get; set; }
        public int LabTechnicianId { get; set; }
        public DateTime LabTestDate { get; set; }
        public LabTestTime LabTestTime { get; set; }  
        public LabTestStatus Status { get; set; }  // Scheduled/Cancelled/Completed
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = "Cash";  // Cash/Card/UPI
        public PaymentStatus PaymentStatus { get; set; }  // Pending/Success/...
       
       
    }
}
