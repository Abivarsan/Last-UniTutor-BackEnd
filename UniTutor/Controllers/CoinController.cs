using Microsoft.AspNetCore.Mvc;
using UniTutor.DTO;
using UniTutor.Interface;

namespace UniTutor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoinController : ControllerBase
    {
        private readonly IInvitationService _invitationService;

        public CoinController(IInvitationService invitationService)
        {
            _invitationService = invitationService;
        }

        [HttpPost("verify-code")]
        public IActionResult VerifyCode([FromBody] VerifyCodeRequestDto request)
        {
            bool result = _invitationService.VerifyCode(request);
            if (result)
            {
                return Ok("Coins rewarded successfully.");
            }
            return BadRequest("Invalid or already used verification code.");
        }
    }
}
