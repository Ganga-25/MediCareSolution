using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Domain.Enums
{
    public enum ScheduleStatus
    {
        Pending=0,
        Rescheduled = 1,
        Cancelled = 2,
        Completed=3
    }
}
