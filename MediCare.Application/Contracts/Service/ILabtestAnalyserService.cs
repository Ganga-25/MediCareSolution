using MediCare.Application.DTOs.LabTestDTO;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Application.Contracts.Service
{
    public interface ILabtestAnalyserService
    {
        Task<LabTestAnalysisResponseDTO> AnalyzeLabReportAsync(IFormFile file);
    }
}
