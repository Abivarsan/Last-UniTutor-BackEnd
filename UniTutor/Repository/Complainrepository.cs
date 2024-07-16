//using Microsoft.EntityFrameworkCore;
//using UniTutor.Model;

//namespace UniTutor.Repository
//{
//    public class Complainrepository
//    {
//        public async Task<Comment> AddCommentAsync(Comment comment)
//        {
//            comment.CreatedAt = DateTime.UtcNow;
//            _context.Comments.Add(comment);
//            await _context.SaveChangesAsync();
//            return comment;
//        }

//        public async Task<IEnumerable<Comment>> GetCommentsByStudentIdAsync(int studentId)
//        {
//            return await _context.Comments.Where(c => c.StudentId == studentId).ToListAsync();
//        }
//    }
//}
