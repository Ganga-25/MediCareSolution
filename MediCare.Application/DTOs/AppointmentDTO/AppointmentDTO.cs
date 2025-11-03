using MediCare.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Application.DTOs.AppointmentDTO
{
    public class AppointmentDTO
    {
       
        public int DoctorId { get; set; }

        public DateTime AppointmentDate { get; set; }
        public String AppointmentTime { get; set; } = null!;

        public Mode Mode { get; set; }
        public string? Notes { get; set; }
       
    }
}
