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
        [HttpPost]
        /*[Authorize(Roles = "Doctor,Admin")]*/ // restrict if needed
        public async Task<IActionResult> Add([FromBody] StaffAvailabilityCreateUpdateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<string>(400, "Invalid data"));

            // Get current user info from JWT token
            var currentUserId = User.GetUserId();
            var role = User.GetUserRole();

            var response = await _staffAvaiService.AddAsync(dto, currentUserId, role);
            return StatusCode(response.StatusCode, response);
        }

        // ✅ PUT: api/StaffAvailability/{id}
        [HttpPut("{id}")]
        
        public async Task<IActionResult> Update(int id, [FromBody] StaffAvailabilityCreateUpdateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<string>(400, "Invalid data"));

            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
            var role = User.FindFirstValue(ClaimTypes.Role) ?? "Doctor";

            var response = await _staffAvaiService.UpdateAsync(id, dto, currentUserId, role);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("staff/{staffId}")]
        public async Task<IActionResult> GetByStaffId(int staffId)
        {
            var response = await _staffAvaiService.GetByStaffIdAsync(staffId);
            return StatusCode(response.StatusCode, response);
        }
    }
}

