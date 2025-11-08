using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Application.DTOs.LabTestDTO
{
    public class LabTestAnalysisResponseDTO
    {
        public string Summary { get; set; } = string.Empty;
        public bool IsAbnormal { get; set; }
        public string Suggestions { get; set; } = string.Empty;
    }
    public class FileUploadRequest
    {
        public IFormFile File { get; set; }
    }

}
