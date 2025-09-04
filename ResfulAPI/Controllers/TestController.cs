using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyProject.Application.Services;
using MyProject.Application.Services.Interfaces;
using MyProject.Domain.Entities;
namespace ResfulAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly INguyenService _nguyenService;
        private readonly ITesttiepService _testtiepService;
        public TestController(INguyenService nguyenService, ITesttiepService testtiepService)
        {
            _nguyenService = nguyenService;
            _testtiepService = testtiepService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync(int pageNumber, int pageSize)
        {
            var data = await _nguyenService.GetAllAsync(pageNumber, pageSize);
            return Ok(data);
        }


        [HttpPost]
        public async Task<IActionResult> PostAsync(NguyenEntity tesst)
        {
            var created = await _nguyenService.CreateAsync(tesst);
            return Ok(created);
        }
        [HttpGet("Testtiep")]
        public async Task<IActionResult> GetTesttiepAsync()
        {
            var data = await _testtiepService.GetAllAsync();
            return Ok(data);
        }

        [HttpPost("Testtiep")]
        public async Task<IActionResult> PostTesttiepAsync(Testtiep entity)
        {
            var created = await _testtiepService.CreateAsync(entity);
            return Ok(created);
        }
    }
}
