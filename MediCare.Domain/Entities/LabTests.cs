using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Domain.Entities
{
    public class LabTests:BaseEntity
    {
        public int LabTestId { get; set; }
        public string TestName { get; set; } = null!;
        public int DepartmentId { get; set; }
        public string? Descriptions { get; set; }
        public decimal Price { get; set; }
        public TimeSpan TestCompletionTime { get; set; }
        public int SlotsPerDay { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
