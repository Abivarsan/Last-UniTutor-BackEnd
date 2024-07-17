using Microsoft.AspNetCore.Mvc;
using UniTutor.Interface;
using UniTutor.Repository;

namespace UniTutor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartDataController : ControllerBase
    {
        private readonly IChartData _repository;

        public ChartDataController(IChartData repository)
        {
            _repository = repository;
        }

        [HttpGet("monthly")]
        public async Task<IActionResult> GetMonthlyChartData()
        {
            var data = await _repository.GetMonthlyChartDataAsync();
            return Ok(data);
        }
    }
}

