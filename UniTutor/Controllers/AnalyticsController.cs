
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UniTutor.Interface;

namespace UniTutor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnalyticsController : ControllerBase
    {
        private readonly IAnalytics _analyticsRepository;

        public AnalyticsController(IAnalytics analyticsRepository)
        {
            _analyticsRepository = analyticsRepository;
        }

        [HttpGet("weekly-joined-tutors")]
        public async Task<IActionResult> GetWeeklyJoinedTutors()
        {
            var data = await _analyticsRepository.GetWeeklyJoinedTutorsAsync();
           // var verifiedTutors = data.Where(tutor => tutor.Verified==true);
            return Ok(data);
        }

        [HttpGet("weekly-joined-students")]
        public async Task<IActionResult> GetWeeklyJoinedStudents()
        {
            var data = await _analyticsRepository.GetWeeklyJoinedStudentsAsync();
            return Ok(data);
        }

        [HttpGet("weekly-tutor-requests")]
        public async Task<IActionResult> GetWeeklyTutorRequests()
        {
            var data = await _analyticsRepository.GetWeeklyTutorRequestsAsync();
            return Ok(data);
        }

        [HttpGet("weekly-comments")]
        public async Task<IActionResult> GetWeeklyComments()
        {
            var data = await _analyticsRepository.GetWeeklyCommentsAsync();
            return Ok(data);
        }
    }
}
