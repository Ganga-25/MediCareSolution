using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Application.DTOs.DepartmentDTO
{
    public class DepartmentDTO
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = null!;
        public string Descriptions { get; set; } = string.Empty;
        public string DepartmentImageUrl { get; set; } = string.Empty;

    }
}
