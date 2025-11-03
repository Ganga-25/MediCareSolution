using MediCare.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Application.DTOs.AppointmentDTO
{
    public class AppointmentDetailsDTO
    {
        public int AppointmentId { get; set; }

        public int PatientId { get; set; }
        public string UHID { get; set; } 
        public int DoctorId { get; set; }

        public DateTime AppointmentDate { get; set; }
        public string AppointmentTime { get; set; }

        public Mode Mode { get; set; }
        public string? Notes { get; set; }
        public AppointmentStatus AppointmentStatus { get; set; }


        public Reschedule_Type? RescheduleType { get; set; }
        public string? RescheduleReason { get; set; }

        public ScheduleStatus? RescheduleStatus { get; set; }
        public DateTime? OldDate { get; set; }
        public DateTime? NewDate { get; set; }
        public string? OldTimeSlot { get; set; }
        public string? NewTimeSlot { get; set; }
        public int? OldDoctorId { get; set; }
        public int? NewDoctorId { get; set; }


    }
}
