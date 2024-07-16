using Microsoft.EntityFrameworkCore;
using UniTutor.DataBase;
using UniTutor.Interface;
using UniTutor.Model;

namespace UniTutor.Repository
{
    public class UserRepository : IUser
    {
        private readonly ApplicationDBContext _context;

        public UserRepository(ApplicationDBContext context)
        {
            _context = context;
        }


        public async Task<Tutor> GetTutorByIdAsync(int id)
        {
            return await _context.Tutors.FindAsync(id);
        }

        public async Task<Student> GetStudentByIdAsync(int id)
        {
            return await _context.Students.FindAsync(id);
        }

        public async Task DeleteTutorAsync(int tutorId)
        {
            var tutor = await _context.Tutors
                .Include(t => t.TodoItems)
                .Include(t => t.Comments)
                .Include(t => t.Subjects)
                .Include(t => t.Requests)
                .ThenInclude(r => r.Subject) // Ensure subject is included
                .Include(t => t.Transactions) // Include Transactions
                .FirstOrDefaultAsync(t => t._id == tutorId);

            if (tutor == null)
            {
                throw new ArgumentException($"Tutor with id {tutorId} not found.");
            }

            // Remove Transactions first
            _context.Transactions.RemoveRange(tutor.Transactions);

            // Remove Requests next, as they have dependencies on Subjects
            foreach (var request in tutor.Requests)
            {
                // Remove related subjects if needed
                if (request.Subject != null)
                {
                    _context.Requests.Remove(request);
                    // Optionally remove the Subject if it's only associated with this Request
                    if (!_context.Requests.Any(r => r.subjectId == request.subjectId))
                    {
                        _context.Subjects.Remove(request.Subject);
                    }
                }
                else
                {
                    _context.Requests.Remove(request);
                }
            }

            // Remove other related entities
            _context.Comments.RemoveRange(tutor.Comments);
            _context.TodoItems.RemoveRange(tutor.TodoItems);

            // Remove tutor's subjects that are not already removed
            _context.Subjects.RemoveRange(tutor.Subjects);

            // Finally, remove the tutor
            _context.Tutors.Remove(tutor);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteStudentAsync(int studentId)
        {
            var student = await _context.Students
                .Include(s => s.TodoItems)
                .Include(s => s.Comments)
                .Include(s => s.Requests)

                


                .FirstOrDefaultAsync(s => s._id == studentId);

            if (student == null)
            {
                throw new ArgumentException($"Student with id {studentId} not found.");
            }

            // Remove related TodoItems
            _context.TodoItems.RemoveRange(student.TodoItems);

            _context.Comments.RemoveRange(student.Comments);

            _context.Requests.RemoveRange(student.Requests);


            // Remove student
            _context.Students.Remove(student);

            await _context.SaveChangesAsync();
        }

    }
}
