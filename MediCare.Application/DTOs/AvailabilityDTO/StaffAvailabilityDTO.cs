using MediCare.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Application.DTOs.AvailabilityDTO
{
    public class StaffAvailabilityDTO
    {
        public int Id { get; set; }
        public StaffType StaffType { get; set; }
        public int StaffId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int AvailableSlots { get; set; }
        public int? SlotDuration { get; set; }
        public string? SlotList { get; set; } // 07:00 AM,07:10 AM,...
        public string? Session { get; set; }
        public Mode? Mode { get; set; }
    }
    public class StaffAvailabilityCreateUpdateDTO
    {
       
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int AvailableSlots { get; set; }
        public int? SlotDuration { get; set; }
        public Mode? Mode { get; set; }
        public string? SlotList { get; set; } // 07:00 AM,07:10 AM,...
        public string? Session { get; set; }
    }
}
