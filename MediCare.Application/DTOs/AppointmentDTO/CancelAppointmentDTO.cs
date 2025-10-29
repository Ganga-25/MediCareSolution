using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Application.DTOs.AppointmentDTO
{
    public class CancelAppointmentDTO
    {
        public int AppointmentId { get; set; }
        public string? CancellationReason { get; set; }

    }
}
