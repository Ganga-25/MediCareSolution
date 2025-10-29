using MediCare.Application.Common;
using MediCare.Application.Contracts.Service;
using MediCare.Application.DTOs.AppointmentDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MediCare.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }
        [HttpPost]
        public async Task<IActionResult> AddAppointment([FromBody] AppointmentDTO dto)
        {
            var result= await _appointmentService.BookAppointmentAsync(dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAppointments()
        {
            var response = await _appointmentService.GetAllAppointmentsAsync();
            return StatusCode(response.StatusCode, response);
        }

        // ✅ GET: /api/appointment/patient
        [HttpGet("patient")]
        public async Task<IActionResult> GetAppointmentsForPatient()
        {
            var userIdClaim = User.FindFirst("userid")?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("UserId not found in token.");

            int userId = int.Parse(userIdClaim);

            var response = await _appointmentService.GetAppointmentsByPatientIdAsync(userId);
            return StatusCode(response.StatusCode, response);
        }

        // ✅ GET: /api/appointment/doctor/{doctorId}
        [HttpGet("doctor/{doctorId}")]
        public async Task<IActionResult> GetAppointmentsForDoctor(int doctorId)
        {
            var response = await _appointmentService.GetAppointmentsByDoctorIdAsync(doctorId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("cancel")]
        public async Task<IActionResult> CancelAppointment([FromBody] CancelAppointmentDTO dto)
        {
            // Get values from JWT (your custom claim keys)
            var userIdClaim = User.FindFirstValue("userId");
            var roleClaim = User.FindFirstValue("userRole");

            if (string.IsNullOrEmpty(userIdClaim))
                return BadRequest(new { message = "User ID missing in token" });

            int userId = int.Parse(userIdClaim);
            string role = roleClaim ?? "Patient";

            var response = await _appointmentService.CancelAppointmentAsync(dto, userId, role);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("reschedule")]
        [Authorize(Roles = "Patient,Admin")]
        public async Task<IActionResult> RescheduleAppointment([FromBody] RescheduleAppointmentDTO dto)
        {
            // ✅ Get userId and role from JWT token
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var roleClaim = User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || string.IsNullOrEmpty(roleClaim))
                return Unauthorized(new ApiResponse<string>(401, "Unauthorized access"));

            int userId = int.Parse(userIdClaim);
            string role = roleClaim;

            // ✅ Call service
            var response = await _appointmentService.RescheduleAppointmentAsync(dto, userId, role);

            // ✅ Return proper HTTP response
            return StatusCode(response.StatusCode, response);
        }
    }

}

