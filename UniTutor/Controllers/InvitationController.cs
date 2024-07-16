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
            bool invitationSent = _invitationService.InviteFriend(request);

            if (invitationSent)
            {
                return Ok();
            }
            else
            {
                return BadRequest("Invitation failed: Email is already registered as a tutor."); 
            }
        }
    }
}
