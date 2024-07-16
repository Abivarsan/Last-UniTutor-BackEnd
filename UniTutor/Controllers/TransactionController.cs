//using Microsoft.AspNetCore.Mvc;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using UniTutor.Interface;
//using UniTutor.Model;

//namespace UniTutor.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class TransactionController : ControllerBase
//    {
//        private readonly ITransaction _transaction;

//        public TransactionController(ITransaction transaction)
//        {
//            _transaction = transaction;
//        }

//        [HttpGet("getall/{tutorId}")]
//        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactionsByTutorId(int tutorId)
//        {
//            var transactions = await _transaction.GetTransactionsByTutorId(tutorId);

//            var result = transactions.Select(t => new
//            {
//                t.Id,
//                t.TransactionTime,
//                t.Description,
//                t.Coins
//                // Add more properties as needed
//            }).ToList();

//            return Ok(result);
//        }
//        [HttpGet("totalamount/{tutorId}")]
//        public async Task<ActionResult<decimal>> GetTotalAmountByTutorId(int tutorId)
//        {
//            var transactions = await _transaction.GetTransactionsByTutorId(tutorId);
//            decimal totalAmount = transactions.Sum(t => t.Coins);

//            return Ok(totalAmount);
//        }
//    }
//}
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniTutor.Interface;
using UniTutor.Model;
using UniTutor.Repository;

namespace UniTutor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransaction _transaction;
        private readonly ITutor _tutor; // Assuming you have a repository for tutors

        public TransactionController(ITransaction transaction, ITutor tutor)
        {
            _transaction = transaction;
            _tutor = tutor;
        }

        [HttpGet("getall/{tutorId}")]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactionsByTutorId(int tutorId)
        {
            var transactions = await _transaction.GetTransactionsByTutorId(tutorId);
            var slstTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Colombo");
            var slstTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, slstTimeZone);

            
            var result = transactions.Select(t => new
            {
                t.Id,
                t.timestamp,
                t.Description,
                t.Coins
                // Add more properties as needed
            }).ToList();

            return Ok(result);
        }

        [HttpGet("totalamount/{tutorId}")]
        public async Task<ActionResult<decimal>> GetTotalAmountByTutorId(int tutorId)
        {
            try
            {
                var transactions = await _transaction.GetTransactionsByTutorId(tutorId);
                var tutor = await _tutor.GetByIdAsync(tutorId); // Fetch tutor by Id

                if (tutor == null)
                {
                    return NotFound($"Tutor with ID {tutorId} not found.");
                }

                decimal tutorCoins = tutor.Coins; // Get tutor's coins from the tutor entity
                decimal totalTransactionCoins = transactions.Sum(t => t.Coins); // Sum of transaction coins

                decimal totalAmount = tutorCoins + totalTransactionCoins; // Total amount including tutor's coins

                return Ok(totalAmount);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Additional method to get tutor by ID (assuming you have a repository method for this)
        [HttpGet("gettutor/{tutorId}")]
        public async Task<ActionResult<Tutor>> GetTutorById(int tutorId)
        {
            var tutor = await _tutor.GetByIdAsync(tutorId);

            if (tutor == null)
            {
                return NotFound($"Tutor with ID {tutorId} not found.");
            }

            return Ok(tutor);
        }
    }
}
