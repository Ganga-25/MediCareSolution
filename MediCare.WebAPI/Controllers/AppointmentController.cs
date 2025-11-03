using MediCare.Application.Common;
using MediCare.Application.Contracts.Service;
using MediCare.Application.DTOs.AppointmentDTO;
using MediCare.Infrastructure.Extentions;
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
            int userId=User.GetUserId();
            var result= await _appointmentService.BookAppointmentAsync(dto,userId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAppointments()
        {
            var response = await _appointmentService.GetAllAppointmentsAsync();
            return StatusCode(response.StatusCode, response);
        }

  
        [HttpGet("patient")]
        public async Task<IActionResult> GetAppointmentsForPatient()
        {
           int userId=User.GetUserId();

            var response = await _appointmentService.GetAppointmentsByPatientIdAsync(userId);
            return StatusCode(response.StatusCode, response);
        }

        
        [HttpGet("doctor/{doctorId}")]
        public async Task<IActionResult> GetAppointmentsForDoctor(int doctorId)
        {
            var response = await _appointmentService.GetAppointmentsByDoctorIdAsync(doctorId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("cancel")]
        public async Task<IActionResult> CancelAppointment([FromBody] CancelAppointmentDTO dto)
        {

            int userId = User.GetUserId();
            string role = User.GetUserRole();

            var response = await _appointmentService.CancelAppointmentAsync(dto, userId, role);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("reschedule")]
        //[Authorize(Roles = "Patient,Admin")]
        public async Task<IActionResult> RescheduleAppointment([FromBody] RescheduleAppointmentDTO dto)
        {
                       
            int userId = User.GetUserId();
            string role = User.GetUserRole();
            
            var response = await _appointmentService.RescheduleAppointmentAsync(dto, userId, role);      
            return StatusCode(response.StatusCode, response);
        }
    }

}

