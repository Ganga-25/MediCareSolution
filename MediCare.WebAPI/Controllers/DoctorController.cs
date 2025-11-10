using MediCare.Application.Contracts.Service;
using MediCare.Application.DTOs.ProfileUpdateDTO;
using MediCare.Application.ServiceImplementations;
using MediCare.Domain.Entities;
using MediCare.Domain.Enums;
using MediCare.Infrastructure.Extentions;
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

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _doctorService.GetByIdAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] DoctorUpdateDTO Docdto)
        {
          
           int  userId = User.GetUserId();
            var result = await _doctorService.RegisterDoctorAsync(Docdto, userId);
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


        [HttpPut("verify")]
        public async Task<IActionResult> VerifyDoctor(DoctorVerificationUpdateDTO dto)
        {
          int userId= User.GetUserId();

            var response = await _doctorService.UpdateDoctorVerificationStatusAsync(dto,userId);

            return StatusCode(response.StatusCode, response);
        }





    }
}
