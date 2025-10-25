using MediCare.Application.Contracts.Service;
using MediCare.Application.DTOs.ProfileUpdateDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediCare.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _doctorService;
        public  DoctorController(IDoctorService doctorService)
        {
            _doctorService=doctorService;

        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _doctorService.GetAllAsync();
            return StatusCode(response.StatusCode, response);
        }

        // ✅ GET: api/doctors/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _doctorService.GetByIdAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] DoctorUpdateDTO Docdto, int userId)
        {
            // 1️⃣ Extract UserId from JWT claims
            //var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            //if (string.IsNullOrEmpty(userIdClaim))
            //    return Unauthorized(new { message = "UserId not found in token" });

            //if (!int.TryParse(userIdClaim, out int userId))
            //    return BadRequest(new { message = "Invalid UserId in token" });

            // 2️⃣ Call the service method
            var result = await _doctorService.RegisterDoctorAsync(Docdto, userId);

            // 3️⃣ Return appropriate HTTP status code and response
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("department/{departmentId}/doctors")]
        public async Task<IActionResult> GetDoctorsByDepartment(int departmentId)
        {
            var response = await _doctorService.GetDoctorsByDepartmentAsync(departmentId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingDoctors()
        {
            var response = await _doctorService.GetPendingDoctorsAsync();
            return StatusCode(response.StatusCode, response);
        }

    }
}
