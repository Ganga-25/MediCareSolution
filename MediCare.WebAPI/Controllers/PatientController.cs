using MediCare.Application.Contracts.Service;
using MediCare.Application.DTOs.ProfileUpdateDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediCare.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;
        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;

        }
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] PatientUpdateDTO Patdto, int userId)
        {
            // 1️⃣ Extract UserId from JWT claims
            //var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            //if (string.IsNullOrEmpty(userIdClaim))
            //    return Unauthorized(new { message = "UserId not found in token" });

            //if (!int.TryParse(userIdClaim, out int userId))
            //    return BadRequest(new { message = "Invalid UserId in token" });

            // 2️⃣ Call the service method
            var result = await _patientService.RegisterPatientAsync(Patdto, userId);

            // 3️⃣ Return appropriate HTTP status code and response
            return StatusCode(result.StatusCode, result);
        }
    }
}
