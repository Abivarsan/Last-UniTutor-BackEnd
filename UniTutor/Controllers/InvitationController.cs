using Microsoft.AspNetCore.Mvc;
using UniTutor.DTO;
using UniTutor.Interface;

namespace UniTutor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvitationController : ControllerBase
    {
        private readonly IInvitationService _invitationService;

        public InvitationController(IInvitationService invitationService)
        {
            _invitationService = invitationService;
        }

        [HttpPost("invite")]
        public IActionResult InviteFriend([FromBody] InviteRequestDto request)
        {
            try
            {
                _invitationService.InviteFriend(request);
                return Ok(new { Message = "Invitation sent successfully." });
            }
            catch (InvalidOperationException ex)
            {
                
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, new { Error = "An unexpected error occurred. Please try again later." });
            }
        }
    }
}
