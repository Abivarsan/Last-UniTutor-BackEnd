using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using UniTutor.Repository;
using UniTutor.Respository;
using UniTutor.Interface;

namespace UniTutor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrentUsersTotalController : ControllerBase
    {
        private readonly ICurrentUsersTotal _currentUsersTotalRepository;


        public CurrentUsersTotalController(ICurrentUsersTotal currentUsersTotalRepository)
        {
            _currentUsersTotalRepository = currentUsersTotalRepository;
        }

        [HttpGet("total-students")]
        public async Task<IActionResult> GetTotalStudents()
        {
            var totalStudents = await _currentUsersTotalRepository.GetTotalStudentsAsync();
            return Ok(totalStudents);
        }

        [HttpGet("total-tutors")]
        public async Task<IActionResult> GetTotalTutors()
        {
            var totalTutors = await _currentUsersTotalRepository.GetTotalTutorsAsync();
            return Ok(totalTutors);
        }
    }
}

