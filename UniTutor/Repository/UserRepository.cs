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

        public async Task DeleteTutorAsync(Tutor tutor)
        {
            _context.Tutors.Remove(tutor);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteStudentAsync(int studentId)
        {
            var student = await _context.Students
                .Include(s => s.TodoItems)
                .FirstOrDefaultAsync(s => s._id == studentId);

            if (student == null)
            {
                throw new ArgumentException($"Student with id {studentId} not found.");
            }

            // Remove related TodoItems
            _context.TodoItems.RemoveRange(student.TodoItems);

            // Remove student
            _context.Students.Remove(student);

            await _context.SaveChangesAsync();
        }

    }
}
