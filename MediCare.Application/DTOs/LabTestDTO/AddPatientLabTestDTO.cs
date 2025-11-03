using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Application.DTOs.LabTestDTO
{
    public class AddPatientLabTestDTO
    {
        public int AppointmentId { get; set; }
        public string TestName { get; set; } 
    }
}
