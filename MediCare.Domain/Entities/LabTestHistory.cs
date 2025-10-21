using MediCare.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Domain.Entities
{
    public class LabTestHistory
    {
        public int HistoryId { get; set; }
        public int LabTestId { get; set; }
        public int PatientId { get; set; }
        public Actions Actions { get; set; } 
        public string? OldValues { get; set; }
        public string? NewValues { get; set; }
        public string? ReportFile { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
