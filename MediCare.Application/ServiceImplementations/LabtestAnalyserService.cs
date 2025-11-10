using MediCare.Application.Contracts.Service;
using MediCare.Application.DTOs.LabTestDTO;
using MediCare.Application.ServiceImplementations.TestAnalyzers;
using Microsoft.AspNetCore.Http;
using UglyToad.PdfPig;

namespace MediCare.Application.ServiceImplementations
{
    public class LabtestAnalyserService : ILabtestAnalyserService
    {
        private readonly List<ITestAnalyzer> _analyzers;

        public LabtestAnalyserService()
        {
            _analyzers = new List<ITestAnalyzer>
            {
                new BloodSugarAnalyzer(),
                // Add more analyzers here later
                // new LiverFunctionAnalyzer(),
                // new ThyroidAnalyzer()
            };
        }

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

            // Try to detect the right analyzer
            var analyzer = _analyzers.FirstOrDefault(a => a.CanAnalyze(text));

            if (analyzer != null)
            {
                return analyzer.Analyze(text);
            }

            // Default if no analyzer matches
            return new LabTestAnalysisResponseDTO
            {
                Summary = "Unknown report type.",
                IsAbnormal = false,
                Suggestions = "Please upload a supported lab test (e.g., Blood Sugar, Liver, Thyroid)."
            };
        }
    }
}
