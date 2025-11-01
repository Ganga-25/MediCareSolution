using MediCare.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Domain.Entities
{
    public class Appointment:BaseEntity
    {
        public int AppointmentId { get; set; }

        public int PatientId { get; set; }
        public string UHID { get; set; } = null!;
        public int DoctorId { get; set; }

        public DateTime AppointmentDate { get; set; }
        public TimeSpan AppointmentTime { get; set; }

        public Mode Mode { get; set; }
        public string? Notes { get; set; }
        public AppointmentStatus AppointmentStatus { get; set; } = AppointmentStatus.Scheduled;
       

        public CancelledBy? CancellationType { get; set; }
        public string? CancellationReason { get; set; }

        public Reschedule_Type? RescheduleType { get; set; }
        public string? RescheduleReason { get; set; }

        public ScheduleStatus? RescheduleStatus { get; set; }
        public DateTime? OldDate { get; set; }
        public DateTime? NewDate { get; set; }
        public TimeSpan? OldTimeSlot { get; set; }
        public TimeSpan? NewTimeSlot { get; set; }
        public int? OldDoctorId { get; set; }
        public int? NewDoctorId { get; set; }
               
    }
}