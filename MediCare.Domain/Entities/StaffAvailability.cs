using MediCare.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Domain.Entities
{
    public class StaffAvailability:BaseEntity
    {
        public int Id { get; set; }
        public StaffType StaffType { get; set; }
        public int StaffId { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public int AvailableSlots { get; set; }
        public int? SlotDuration { get; set; }
        public Mode? Mode { get; set; }

    }
}
