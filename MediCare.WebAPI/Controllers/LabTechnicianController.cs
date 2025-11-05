using MediCare.Application.Contracts.Service;
using MediCare.Application.DTOs.ProfileUpdateDTO;
using MediCare.Infrastructure.Extentions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediCare.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabTechnicianController : ControllerBase
    {
        private readonly ILabtechnicianService _labTechService;

        public LabTechnicianController(ILabtechnicianService labTechService)
        {
            _labTechService = labTechService;
        }

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] LabTechnicianUpdateDTO labTechDto)
        {
            int userId=User.GetUserId();
            var result = await _labTechService.RegisterLabTechnicianAsync(labTechDto, userId);
            return StatusCode(result.StatusCode, result);
        }
        [HttpGet("PendingVerification")]
        public async Task<IActionResult> GetpendingLabtech()
        {
            var result = await _labTechService.GetPendingVerificationLabtech();
                return StatusCode(result.StatusCode, result);
        }
        [HttpPatch("Verify")]
        public async Task<IActionResult> UpdateVerification([FromBody] UpdateverificationstatusofLabtechDTO dto)
        {
            int userId=User.GetUserId();
            var result = await _labTechService.UpdateLabtechnicianVerficationStatus(dto,userId);
            return StatusCode(result.StatusCode, result);


        }
        [HttpGet]
        public async Task<IActionResult> GetLabtechnician()
        {
            int userId = User.GetUserId();
            var result= await _labTechService.GetLabTechByUserIdAsync(userId);
            return StatusCode(result.StatusCode, result);
        }
       

    }
}

