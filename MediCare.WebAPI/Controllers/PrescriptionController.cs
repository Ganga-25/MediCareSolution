using MediCare.Application.Contracts.Service;
using MediCare.Application.DTOs.PrescriptionDTO;
using MediCare.Infrastructure.Extentions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediCare.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrescriptionController : ControllerBase
    {
        private readonly IPrescriptionService _prescriptionService;
        public PrescriptionController(IPrescriptionService prescriptionService)
        {
           _prescriptionService = prescriptionService;
        }

        [HttpPost]
        public async Task<IActionResult> AddPrescription([FromBody] AddPresciptionDTO addPresciptionDTO)
        {
            int userId= User.GetUserId();
            string userRole=User.GetUserRole();
            var result= await _prescriptionService.AddPrescription(addPresciptionDTO, userId, userRole);
            return StatusCode(result.StatusCode, result);
        }
    }
}
