// Controllers/UserController.cs
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniTutor.Interface;
using UniTutor.Respository;

namespace UniTutor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LastJoinedController : ControllerBase
    {
        private readonly ILastJoined _lastjoinedRepository;

        public LastJoinedController(ILastJoined lastjoinedRepository)
        {
            _lastjoinedRepository = lastjoinedRepository;
        }

        [HttpGet("last-joined")]
        public async Task<IActionResult> GetLastJoinedUsers([FromQuery] int count = 6)
        {
            var users = await _lastjoinedRepository.GetLastJoinedUsersAsync(count);
            return Ok(users);
        }
    }
}