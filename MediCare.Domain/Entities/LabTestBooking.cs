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
        public DateOnly LabTestDate { get; set; }
        public LabTestTime LabTestTime { get; set; }  
        public LabTestStatus Status { get; set; }  // Scheduled/Cancelled/Completed
        public decimal Amount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }  // Cash/Card/UPI
        public PaymentStatus PaymentStatus { get; set; }  // Pending/Success/...
        public string TransactionId { get; set; } = string.Empty;
        public string PaymentGateway { get; set; } = null!;
        public decimal? RefundAmount { get; set; }
        public string? RefundReason { get; set; }
        public RefundStatus? RefundStatus { get; set; }
        public int? RescheduledBy { get; set; }
        public string? RescheduleReason { get; set; }
        public DateOnly? OldDate { get; set; }
        public DateOnly? NewDate { get; set; }
        public TimeOnly? OldTestTime { get; set; }
        public TimeOnly? NewTestTime { get; set; }
    }
}
