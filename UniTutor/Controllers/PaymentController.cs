using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Stripe;
using Stripe.Checkout;
using UniTutor.DTO;
using UniTutor.Interface;

namespace UniTutor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }
        [HttpPost("create-checkout-session/{id}")]
        public IActionResult CreateCheckoutSession([FromBody] CreateCheckoutSessionDto createCheckoutSessionDto, int id)
        {
            // Convert id to string
            var userId = id.ToString();

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not authenticated.");
            }

            var sessionId = _paymentService.CreateCheckoutSession(int.Parse(userId), createCheckoutSessionDto);
            return Ok(new { SessionId = sessionId });
        }

        [HttpGet("checkout-session-status/{sessionId}")]
        public IActionResult GetCheckoutSessionStatus(string sessionId)
        {
            var sessionService = new SessionService();
            var session = sessionService.Get(sessionId);

            return Ok(new { session.Status });
        }
    }
}
