using MediCare.Application.Contracts.Service;
using MediCare.Application.DTOs.LabTestDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediCare.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabTestAnalyzerController : ControllerBase
    {
        private readonly ILabtestAnalyserService _labtestAnalyserService;

        public LabTestAnalyzerController(ILabtestAnalyserService labtestAnalyserService)
        {
            _labtestAnalyserService = labtestAnalyserService;
        }

        /// <summary>
        /// Uploads a lab test report (PDF) and analyzes it.
        /// </summary>
        /// <param name="file">PDF file to analyze</param>
        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadReport([FromForm] FileUploadRequest request)
        {
            var file = request.File;

            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var result = await _labtestAnalyserService.AnalyzeLabReportAsync(file);
            return Ok(result);
        }

    }
}

