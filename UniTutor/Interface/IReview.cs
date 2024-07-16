using UniTutor.Model;

namespace UniTutor.Interface
{
    public interface IReview
    {
        Task<double> GetAverageRatingBySubjectIdAsync(int subjectId);
        Task<Review> GetReviewByStudentAndSubjectAsync(int studentId, int subjectId);
        Task<List<Review>> GetReviewsBySubjectIdAsync(int subjectId);
        Task AddReviewAsync(Review review);
        Task<Review> DeleteReviewAsync(int id);
        Task<Review> UpdateReviewAsync(int id, int studentId, Review review);
    }
}
