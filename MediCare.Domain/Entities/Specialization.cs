using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Domain.Entities
{
    public class Specialization:BaseEntity
    {
        public int SpecializationId { get; set; }
        public string SpecializationName { get; set; } = null!;
        public int DepartmentId { get; set; }

    }
}
