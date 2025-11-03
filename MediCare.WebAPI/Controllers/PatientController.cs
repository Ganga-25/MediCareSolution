using MediCare.Application.Contracts.Service;
using MediCare.Application.DTOs.ProfileUpdateDTO;
using MediCare.Infrastructure.Extentions;
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
        public async Task<IActionResult> UpdateProfile([FromBody] PatientUpdateDTO Patdto)
        {
           int userId=User.GetUserId();
            var result = await _patientService.RegisterPatientAsync(Patdto, userId);

           
            return StatusCode(result.StatusCode, result);
        }
    }
}
