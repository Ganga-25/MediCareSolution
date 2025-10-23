using MediCare.Application.Contracts.Service;
using MediCare.Application.DTOs.LabTestDTO;
using MediCare.Application.ServiceImplementations;
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
            var result= await _testService.GetAllLabTestsAsync();
            return StatusCode(result.StatusCode, result);
        }
       

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result= await _testService.GetLabTestByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }
            

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddLabTestDTO dto)
        {
            var result= await _testService.AddLabTestAsync(dto);
            return StatusCode(result.StatusCode, result);
        }
            

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, [FromQuery] int deletedBy)
        {
            var result=await _testService.DeleteLabTestAsync(id, deletedBy);
            return StatusCode(result.StatusCode, result);

        }
            
    }
}
