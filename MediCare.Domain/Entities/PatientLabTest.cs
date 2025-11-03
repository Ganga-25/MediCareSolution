using MediCare.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Domain.Entities
{
    public class PatientLabTest:BaseEntity
    {
        public int AppointmentId { get; set; }
        public int Id { get; set; }
        public int PatientId { get; set; }   
        public int DoctorId { get; set; }    
        public int DepartmentId { get; set; } 
        public string TestName { get; set; } = null!;
        public bool IsInHouse { get; set; }  
        public LabTestStatus Status { get; set; } = LabTestStatus.Pending;
    }
}
