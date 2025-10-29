using MediCare.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Application.DTOs.AppointmentDTO
{
    public class RescheduleAppointmentDTO
    {
        public int AppointmentId { get; set; }
        public Reschedule_Type RescheduleType { get; set; } // DateAndTime, Doctor, Both
        public DateTime? NewDate { get; set; }
        public TimeSpan? NewTime { get; set; }
        public int? NewDoctorId { get; set; }
        public string? RescheduleReason { get; set; }
    }
}
