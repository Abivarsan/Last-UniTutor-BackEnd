//using Microsoft.EntityFrameworkCore;
//using UniTutor.Interface;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using UniTutor.DataBase;
//using UniTutor.Model;
//using AutoMapper.Internal;

//namespace UniTutor.Respository
//{
//    public class LastJoinedRepository : ILastJoined
//    {
//        private readonly ApplicationDBContext _context;

//        public LastJoinedRepository(ApplicationDBContext context)
//        {
//            _context = context;
//        }

//        public async Task<IEnumerable<object>> GetLastJoinedUsersAsync(int count)
//        {
//            var lastJoinedStudents = await _context.Students
//                .OrderByDescending(s => s.CreatedAt)
//                .Take(count)
//                .Select(s => new { s.Id, FullName = s.firstName + " " + s.lastName, s.email, s.ProfileUrl, s.CreatedAt, Type = "Student" })
//                .ToListAsync();

//            var lastJoinedVerifiedTutors = await _context.Tutors
//               .Where(t => t.Verified)  // Filter for only verified tutors
//               .OrderByDescending(t => t.CreatedAt)
//               .Take(count)
//               .Select(t => new { t.Id, FullName = t.firstName + " " + t.lastName, t.universityMail, t.ProfileUrl, t.CreatedAt, Type = "Tutor" })
//               .ToListAsync();

//            return
//                lastJoinedStudents.Concat(lastJoinedVerifiedTutors)
//                .OrderByDescending(u => u.CreatedAt)
//                .Take(count)
//                .ToList();
//        }

//        public async Task<IEnumerable<Student>> GetStudentsAsync()
//        {
//            return await _context.Students.ToListAsync();
//        }

//        public async Task<IEnumerable<Tutor>> GetTutorsAsync()
//        {
//            return await _context.Tutors.Where(t => t.Verified).ToListAsync();
//        }


//    }
//}
using Microsoft.EntityFrameworkCore;
using UniTutor.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniTutor.DataBase;
using UniTutor.Model;

namespace UniTutor.Repository
{
    public class LastJoinedRepository : ILastJoined
    {
        private readonly ApplicationDBContext _context;

        public LastJoinedRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<object>> GetLastJoinedUsersAsync(int count)
        {
            var lastJoinedStudents = await _context.Students
                .OrderByDescending(s => s.CreatedAt)
                .Take(count)
                .Select(s => new
                {
                    s._id,
                    FullName = s.firstName + " " + s.lastName,
                    s.email,
                    s.ProfileUrl,
                    s.CreatedAt,
                    Type = "Student"
                })
                .ToListAsync();

            var lastJoinedVerifiedTutors = await _context.Tutors
                .Where(t => t.Verified)  // Filter for only verified tutors
                .OrderByDescending(t => t.CreatedAt)
                .Take(count)
                .Select(t => new
                {
                    t._id,
                    FullName = t.firstName + " " + t.lastName,
                    email = t.universityMail,  // Change property name to match the one used in students
                    t.ProfileUrl,
                    t.CreatedAt,
                    Type = "Tutor"
                })
                .ToListAsync();

            return lastJoinedStudents
                .Concat(lastJoinedVerifiedTutors)
                .OrderByDescending(u => u.CreatedAt)
                .Take(count)
                .ToList();
        }

        public async Task<IEnumerable<Student>> GetStudentsAsync()
        {
            return await _context.Students.ToListAsync();
        }

        public async Task<IEnumerable<Tutor>> GetTutorsAsync()
        {
            return await _context.Tutors.Where(t => t.Verified).ToListAsync();
        }
    }
}
