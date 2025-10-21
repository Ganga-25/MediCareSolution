using MediCare.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Domain.Entities
{
    public class LabTestResult:BaseEntity
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public int LabTechnicianId { get; set; }
        public string? Files { get; set; }
        public DateOnly TestDate { get; set; }
        public string TestParameters { get; set; } = string.Empty;
        public string AnalysisResults { get; set; } = string.Empty;
        public ResultStatus Status { get; set; } = ResultStatus.Pending;

    }
}
