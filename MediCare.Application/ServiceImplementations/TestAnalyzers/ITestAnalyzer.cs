using MediCare.Application.DTOs.LabTestDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Application.ServiceImplementations.TestAnalyzers
{
    public interface ITestAnalyzer
    {
        bool CanAnalyze(string text);  // Checks if the report belongs to this test type
        LabTestAnalysisResponseDTO Analyze(string text); // Extracts values and gives results

    }
}
