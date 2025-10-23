using MediCare.Application.Contracts.Service;
using MediCare.Application.DTOs.DepartmentDTO;
using MediCare.Application.DTOs.SpecializationDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediCare.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecializationController : ControllerBase
    {
        private readonly  ISpecializationService _specializationService;
        public SpecializationController(ISpecializationService specializationService)
        {
            _specializationService = specializationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSpecialization()
        {
            var result = await _specializationService.GetAllSpecializationAsync();
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> AddSpecialization([FromBody] AddSpecializationDTO adddto)
        {
            var result = await _specializationService.AddSpecializationAsync(adddto);
            return StatusCode(result.StatusCode, result);

        }
    }
}
