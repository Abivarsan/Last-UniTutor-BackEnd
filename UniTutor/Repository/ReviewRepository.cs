using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UniTutor.DataBase;
using UniTutor.Interface;
using UniTutor.Model;

namespace UniTutor.Repository
{
    public class ReviewRepository: IReview
    {
        private ApplicationDBContext _DBcontext;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public ReviewRepository(ApplicationDBContext DBcontext, IConfiguration config, IMapper mapper)
        {
            _DBcontext = DBcontext;
            _config = config;
            _mapper = mapper;

        }
        public async Task<double> GetAverageRatingBySubjectIdAsync(int subjectId)
        {
            var reviews = await _DBcontext.Reviews
                .Where(r => r.subjectId == subjectId)
                .ToListAsync();

            if (reviews.Count > 0)
            {
                double totalRating = reviews.Sum(r => r.rating);
                return totalRating / reviews.Count;
            }

            return 0;
        }
        

        public async Task<Review> GetReviewByStudentAndSubjectAsync(int studentId, int subjectId)
        {
            return await _DBcontext.Reviews
                .FirstOrDefaultAsync(r => r.studentId == studentId && r.subjectId == subjectId);
        }

        public async Task<List<Review>> GetReviewsBySubjectIdAsync(int subjectId)
        {
            return await _DBcontext.Reviews
                .Where(r => r.subjectId == subjectId)
                .ToListAsync();
        }

        public async Task AddReviewAsync(Review review)
        {
            _DBcontext.Reviews.Add(review);
            await _DBcontext.SaveChangesAsync();
        }

        public async Task<Review> DeleteReviewAsync(int id)
        {
            var review = await _DBcontext.Reviews.FindAsync(id);
            if (review != null)
            {
                _DBcontext.Reviews.Remove(review);
                await _DBcontext.SaveChangesAsync();
            }
            return review;
        }

        public async Task<Review> UpdateReviewAsync(int id, int studentId, Review review)
        {
            if (id != review._id || studentId != review.studentId)
            {
                throw new ArgumentException("Review ID or Student ID does not match.");
            }

            _DBcontext.Entry(review).State = EntityState.Modified;
            await _DBcontext.SaveChangesAsync();
            return review;
        }
    }
}
