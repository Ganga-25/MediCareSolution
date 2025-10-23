using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Application.DTOs.SpecializationDTO
{
    public class AddSpecializationDTO
    {
        public string SpecializationName { get; set; } = null!;
        public int DepartmentId { get; set; }
    }
    public class SpecializationDTO
    {
        public int SpecializationId { get; set; }
        public string SpecializationName { get; set; } = null!;
        public int DepartmentId { get; set; }
    }
}
