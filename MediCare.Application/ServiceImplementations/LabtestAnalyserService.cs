using MediCare.Application.DTOs.LabTestDTO;
using Microsoft.AspNetCore.Http;
using UglyToad.PdfPig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediCare.Application.Contracts.Service;

namespace MediCare.Application.ServiceImplementations
{
    
    
        public class LabtestAnalyserService:ILabtestAnalyserService
        {
            public async Task<LabTestAnalysisResponseDTO> AnalyzeLabReportAsync(IFormFile file)
            {
                string text = string.Empty;

                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    memoryStream.Position = 0;

                    using (var pdf = PdfDocument.Open(memoryStream))
                    {
                        foreach (var page in pdf.GetPages())
                        {
                            text += page.Text + "\n";
                        }
                    }
                }

                // Very basic example logic — you can expand this.
                bool abnormal = text.Contains("High", StringComparison.OrdinalIgnoreCase) ||
                                text.Contains("Low", StringComparison.OrdinalIgnoreCase) ||
                                text.Contains("Abnormal", StringComparison.OrdinalIgnoreCase);

                string tips = abnormal
                    ? "Some readings seem abnormal. Please consult your doctor and stay hydrated."
                    : "All parameters appear within normal range. Keep up your healthy lifestyle!";

                return new LabTestAnalysisResponseDTO
                {
                    Summary = "Lab test analyzed successfully.",
                    IsAbnormal = abnormal,
                    Suggestions = tips
                };
            }
        }
    
}
