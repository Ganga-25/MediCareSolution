using MediCare.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Domain.Entities
{
    public class Patients:BaseEntity
    {
        public int  PatientId { get; set; }
        public string UHID { get; set; } = null!;
        public int UserId { get; set; }
        public int? Age { get; set; }
        public string? ContactNumber { get; set; }
        public Gender? Gender { get; set; }

    }
}
