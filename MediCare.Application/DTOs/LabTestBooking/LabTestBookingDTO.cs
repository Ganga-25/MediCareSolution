using MediCare.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Application.DTOs.LabTestBooking
{
    public class LabTestBookingDTO
    {
        public int Id { get; set; }
        public int LabTestId { get; set; }
        public LabTestTime LabTestTime { get; set; }
        public DateTime LabTestDate { get; set; }
    }
}
