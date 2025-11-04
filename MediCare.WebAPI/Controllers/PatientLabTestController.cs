using MediCare.Application.Contracts.Service;
using MediCare.Application.DTOs.LabTestDTO;
using MediCare.Application.ServiceImplementations;
using MediCare.Infrastructure.Extentions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediCare.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientLabTestController : ControllerBase
    {
         private readonly IPatientLabTestService _patientLabTestService;
       
        public PatientLabTestController(IPatientLabTestService patientLabTestService)
        {
            _patientLabTestService = patientLabTestService;
        }
        [HttpPost]
        public async Task<IActionResult>AddLabTest([FromBody]AddPatientLabTestDTO addLabTestDTO)
        {
            int userId=User.GetUserId();
            string userRole=User.GetUserRole();
            var result=await _patientLabTestService.AddLabtestToPatient(addLabTestDTO, userId, userRole);
            return StatusCode(result.StatusCode, result);

        }
        [HttpGet("/LabTests")]
        public async Task<IActionResult> GetPrescriptionforUser()
        {
            int userId=User.GetUserId();
            var result= await _patientLabTestService.GetLabTestforPatient(userId);
            return StatusCode(result.StatusCode, result);

        }
        
    }
}
