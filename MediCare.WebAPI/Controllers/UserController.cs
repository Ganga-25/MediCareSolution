using MediCare.Application.Contracts;
using MediCare.Application.Contracts.Service;
using MediCare.Application.DTOs;
using MediCare.Application.DTOs.AuthDTO;
using MediCare.Application.ServiceImplementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace MediCare.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // POST: api/Users/register
        [HttpPost("register/Patient")]
        public async Task<IActionResult> RegisterPatient([FromBody] RegisterRequestDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.RegisterPatientAsync(request);

            // Map AuthResponse to proper HTTP status code
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("register/Doctor")]
        public async Task<IActionResult> RegisterDoctor([FromBody] RegisterRequestDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.RegisterDoctorAsync(request);

            // Map AuthResponse to proper HTTP status code
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("register/LabTechnician")]
        public async Task<IActionResult> RegisterLabTech([FromBody] RegisterRequestDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.RegisterLabTechnicianAsync(request);

            // Map AuthResponse to proper HTTP status code
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginUsers([FromBody] LoginRequestDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.LoginAsync(request);

            // Map AuthResponse to proper HTTP status code
            return StatusCode(result.StatusCode, result);
        }
    }



} 

