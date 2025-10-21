using MediCare.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Domain.Entities
{
    internal class AppointmentTransaction
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public int PatientId { get; set; }

        // Transaction Info
        public TransactionType TransactionType { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string TransactionId { get; set; } = null!;
        public string? PaymentGateway { get; set; }

        // Refund Info
        public decimal? RefundAmount { get; set; }
        public RefundStatus? RefundStatus { get; set; }

        // Business Rules
        public int? HoursBeforeAppt { get; set; }
        public int MaxReschedulesAllowed { get; set; } = 5;
    }
}