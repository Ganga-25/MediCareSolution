using MediCare.Application.Contracts.Service;
using MediCare.Application.DTOs.AppointmentDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    }
}
