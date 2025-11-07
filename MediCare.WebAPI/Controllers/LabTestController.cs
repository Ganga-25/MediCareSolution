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
    public class LabTestController : ControllerBase
    {
        private readonly ILabTestService _testService;
        public LabTestController(ILabTestService testService)
        {
            _testService = testService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            string? role = null;
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                role = User.GetUserRole();
            }

            var result = await _testService.GetAllLabTestsAsync(role);
            return StatusCode(result.StatusCode, result);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _testService.GetLabTestByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }


        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddLabTestDTO dto)
        {
            
            var result = await _testService.AddLabTestAsync(dto);
            return StatusCode(result.StatusCode, result);
        }


        [HttpPut]
        public async Task<IActionResult> Update([FromForm] UpdateLabTestDTO dto)
        {
            int userid = User.GetUserId();
            var result = await _testService.UpdateLabTest(dto, userid);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPatch("{id}")]
        public async Task<IActionResult> ActivateorDeactivate(int id)
        {
            int usetid= User.GetUserId();
            var result = await _testService.ActivateorDeactivaeLabtest(id, usetid);
            return StatusCode(result.StatusCode, result);
        }
            
    }
}
