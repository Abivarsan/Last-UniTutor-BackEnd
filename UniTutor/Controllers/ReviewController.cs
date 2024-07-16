using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UniTutor.DTO;
using UniTutor.Interface;
using UniTutor.Model;
using UniTutor.Repository;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UniTutor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        IReview _review;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;


        public ReviewController( IConfiguration config, IMapper mapper, IEmailService emailService,IReview review)
        {
            _config = config;
            _review = review;
            _mapper = mapper;
            _emailService = emailService;
        }
        // GET: api/review/subject/id
        [HttpGet("subjectavgrating/{id}")]
        public async Task<ActionResult<double>> GetAverageRatingBySubjectId(int id)
        {
            try
            {
                var averageRating = await _review.GetAverageRatingBySubjectIdAsync(id);
                return Ok(averageRating);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
       
        // GET: api/review/student/studentId/subject/subjectId
        [HttpGet("student/{studentId}/subject/{subjectId}")]
        public async Task<ActionResult<Review>> GetReviewByStudentAndSubject(int studentId, int subjectId)
        {
            try
            {
                var review = await _review.GetReviewByStudentAndSubjectAsync(studentId, subjectId);
                if (review != null)
                {
                    return Ok(review);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/review/subject/subjectId
        [HttpGet("subjectreview/{subjectId}")]
        public async Task<ActionResult<ReviewResponse>> GetReviewsBySubjectId(int subjectId)
        {
            try
            {
                var reviews = await _review.GetReviewsBySubjectIdAsync(subjectId);

                // Map the retrieved reviews to FeedbackI objects
                var feedbackList = reviews.Select(r => new FeedbackI
                {
                    _id = r._id.ToString(), // Assuming r.Id is of type int or Guid, convert to string
                    studentId = r.studentId.ToString(), // Assuming StudentId is of type int or Guid, convert to string
                    subjectId = r.subjectId.ToString(), // Assuming SubjectId is of type int or Guid, convert to string
                    rating = r.rating,
                    feedback = r.feedback,
                    timestamp = r.timestamp.ToString("yyyy-MM-dd HH:mm"), // Format timestamp as needed
                   // ProfileUrl = r.Student.ProfileUrl,
                    //studentName = r.Student.firstName
                }).ToList();

                // Calculate average rating
                var averageRating = feedbackList.Any() ? feedbackList.Average(f => f.rating) : 0;

                // Create the response object
                var response = new ReviewResponse
                {
                    reviews = feedbackList,
                    averageRating = averageRating
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST: api/review/subject/id
        [HttpPost("create/{subjectid}/{studentid}")]
        public async Task<ActionResult<Review>> CreateReview(int subjectid, int studentid, [FromBody] ReviewDto reviewDto)
        {
            try
            {
                // Convert current UTC time to SLST
                var slstTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Colombo");
                var slstTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, slstTimeZone);
                var review = new Review
                {
                    subjectId = subjectid,
                    studentId = studentid,
                    rating = reviewDto.rating,
                    feedback = reviewDto.feedback,
                    timestamp = slstTime
                };

                await _review.AddReviewAsync(review);

                return CreatedAtAction(nameof(GetReviewByStudentAndSubject), new { studentId = review.studentId, subjectId = review.subjectId }, review);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
       
        

        // DELETE: api/review/id
        [HttpDelete("{id}")]
        public async Task<ActionResult<Review>> DeleteReview(int id)
        {
            try
            {
                var deletedReview = await _review.DeleteReviewAsync(id);
                if (deletedReview == null)
                {
                    return NotFound();
                }
                return Ok(deletedReview);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT: api/review/id/student/studentId
        [HttpPut("{id}/student/{studentId}")]
        public async Task<ActionResult<Review>> UpdateReview(int id, int studentId, [FromBody] Review review)
        {
            try
            {
                var updatedReview = await _review.UpdateReviewAsync(id, studentId, review);
                return Ok(updatedReview);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
