using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniTutor.DTO;
using UniTutor.Interface;

namespace UniTutor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordController : ControllerBase
    {
        private readonly IPasswordService _passwordService;

        public PasswordController(IPasswordService passwordService)
        {
            _passwordService = passwordService;
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest fprequest)
        {
            try
            {
                await _passwordService.SendVerificationCodeAsync(fprequest.Email);
                

               
                return Ok(new { message = "Verification code sent to email" });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        //[HttpPost("reset-password")]
        //public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest rprequest)
        //{
        //    var result = await _passwordService.ResetPasswordAsync(rprequest.Email, rprequest.VerificationCode, rprequest.NewPassword);
        //    if (!result)
        //    {
        //        return BadRequest(new { message = "Invalid verification code" });
        //    }

        //    return Ok(new { message = "Password reset successful" });
        //}
        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequest request)
        {
           
            var result = await _passwordService.VerifyOtpAsync( request.VerificationCode);
            if (result)
            {
                return Ok(new { message = "OTP verified successfully" });
            }

            return BadRequest(new { message = "Invalid OTP or user not found" });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            
            var result = await _passwordService.ResetPasswordAsync(request.NewPassword);
            if (result)
            {
                return Ok(new { message = "Password reset successful" });
            }

            return BadRequest(new { message = "User not found or password reset failed" });
        }

    }
}
