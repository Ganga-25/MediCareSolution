using MediCare.Application.Common;
using MediCare.Application.Contracts.Service;
using MediCare.Application.DTOs.AvailabilityDTO;
using MediCare.Application.ServiceImplementations;
using MediCare.Infrastructure.Extentions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MediCare.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffAvailabilityController : ControllerBase
    {
        private readonly IStaffAvailabilityService _staffAvaiService;
        public StaffAvailabilityController(IStaffAvailabilityService staffAvaiService)
        {
            _staffAvaiService = staffAvaiService;

        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _staffAvaiService.GetAllAsync();
            return StatusCode(response.StatusCode, response);
        }

        // ✅ GET: api/StaffAvailability/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _staffAvaiService.GetByIdAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        // ✅ POST: api/StaffAvailability
        [HttpPost("Doctors")]
        /*[Authorize(Roles = "Doctor,Admin")]*/ // restrict if needed
        public async Task<IActionResult> AddDoctor([FromBody] StaffAvailabilityCreateUpdateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<string>(400, "Invalid data"));

            // Get current user info from JWT token
            int currentUserId = User.GetUserId();
            string role = User.GetUserRole();

            var response = await _staffAvaiService.AddDoctorAvailability(dto, currentUserId, role);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("Labtechnician")]
        public async Task<IActionResult> Addlabtecnician([FromBody] StaffAvailabilityCreateUpdateDTO dto)
        {
            int  currentUserId = User.GetUserId();
            string role = User.GetUserRole();
            var response= await _staffAvaiService.AddLabtechnicianAvailability(dto, currentUserId, role);
            return StatusCode(response.StatusCode, response);

        }

        // ✅ PUT: api/StaffAvailability/{id}
        [HttpPut("{id}")]
        
        public async Task<IActionResult> Update([FromBody] StaffAvailabilityCreateUpdateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<string>(400, "Invalid data"));

            var role = User.GetUserRole();
            var id=User.GetUserId();
            var response = await _staffAvaiService.UpdateAsync(dto, id,role);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("staffId")]
        public async Task<IActionResult> GetByStaffId()
        {
            var staffId=User.GetUserId();
            var response = await _staffAvaiService.GetByStaffIdAsync(staffId);
            return StatusCode(response.StatusCode, response);
        }
    }
}

