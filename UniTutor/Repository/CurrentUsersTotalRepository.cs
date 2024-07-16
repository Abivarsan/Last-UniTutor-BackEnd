using Microsoft.EntityFrameworkCore;
using UniTutor.DataBase;
using UniTutor.Interface;

namespace UniTutor.Respository
{
    public class CurrentUsersTotalRepository : ICurrentUsersTotal
    {
        private readonly ApplicationDBContext _context;

        public CurrentUsersTotalRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<int> GetTotalStudentsAsync()
        {
            return await _context.Students.CountAsync();
        }

        public async Task<int> GetTotalTutorsAsync()
        {
            return await _context.Tutors.CountAsync();
        }
    }
}
