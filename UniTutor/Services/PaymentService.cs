using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using UniTutor.DataBase;
using UniTutor.DTO;
using UniTutor.Model;
using UniTutor.Interface;
using Stripe.Checkout;

namespace UniTutor.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly ITransaction _transactionRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PaymentService(IConfiguration configuration, ITransaction transactionRepository, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _transactionRepository = transactionRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public string CreateCheckoutSession(int userId, CreateCheckoutSessionDto createCheckoutSessionDto)
        {
            var domain = _configuration["AppSettings:Domain"];
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
        {
            new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = createCheckoutSessionDto.Coins * 2000, // Amount in cents (1 coin = 20 LKR)
                    Currency = "lkr",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = "Coins Package",
                    },
                },
                Quantity = 1,
            },
        },
                Mode = "payment",
                SuccessUrl = $"{domain}/Tutor/Coinbank",
                CancelUrl = $"{domain}/Tutor/Coinbank",
            };

            var service = new SessionService();
            var session = service.Create(options);
            var slstTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Colombo");
            var slstTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, slstTimeZone);

            // Save transaction history
            var transaction = new Transaction
            {

                timestamp = slstTime,
                Description = createCheckoutSessionDto.Description,
                Coins = createCheckoutSessionDto.Coins,
                tutorId = userId,
                StripeSessionId = session.Id
            };
            _transactionRepository.AddTransaction(transaction);

            return session.Id;
        }

        public void AddTransaction(TransactionDto transactionDto)

        {
            var slstTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Colombo");
            var slstTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, slstTimeZone);
            var transaction = new Transaction
            {
                timestamp = slstTime,
                Description = transactionDto.Description,
                Coins = transactionDto.Coins,
                tutorId = transactionDto.tutorId // Assuming tutorId is correctly named in TransactionDto
            };

            _transactionRepository.AddTransaction(transaction);
        }
    }
}
