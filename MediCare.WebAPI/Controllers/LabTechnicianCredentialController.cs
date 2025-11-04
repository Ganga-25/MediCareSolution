using MediCare.Application.Contracts.Service;
using MediCare.Application.DTOs.LabTechnicianDTO;
using MediCare.Infrastructure.Extentions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediCare.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabTechnicianCredentialController : ControllerBase
    {
        private readonly ILabTechnicianCredentialService _technicianCredentialService;
        public LabTechnicianCredentialController(ILabTechnicianCredentialService technicianCredentialService)
        {
            _technicianCredentialService= technicianCredentialService;
        }
        [HttpPost]
        public async Task<IActionResult> AddCredential([FromBody] AddLabtechCredentialDTO credentialDTO)
        {
            int userId=User.GetUserId();   
            var result= await _technicianCredentialService.Addasync(credentialDTO,userId);
            return StatusCode(result.StatusCode, result);
        }
        [HttpGet]
        public async Task<IActionResult> GetCredential()
        {
            int userId= User.GetUserId();
            var result= await _technicianCredentialService.GetLabtechnicianCredentialsAsync(userId);
            return StatusCode(result.StatusCode, result);
        }
    }
}
