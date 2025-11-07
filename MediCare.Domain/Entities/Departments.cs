using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Domain.Entities
{
    public class Departments:BaseEntity
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = null!;
        public string Descriptions { get; set; }= string.Empty;
        public string DepartmentImageUrl { get; set; }=string.Empty;
        public bool IsActive { get; set; } = true;
    }
}
