using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UniTutor.DataBase;
using UniTutor.DTO;
using UniTutor.Interface;
using UniTutor.Model;

namespace UniTutor.Repository
{
    public class SubjectRepository : ISubject
    {
        private ApplicationDBContext _DBcontext;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public SubjectRepository(ApplicationDBContext DBcontext, IConfiguration config, IMapper mapper)
        {
            _DBcontext = DBcontext;
            _config = config;
            _mapper = mapper;

        }

        public async Task<bool> CreateSubject(int tutorId, SubjectRequestDto request)
        {
            var tutor = await _DBcontext.Tutors.FindAsync(tutorId);
            if (tutor == null)
            {
                return false; // Tutor not found
            }

            var subject = new Subject
            {
                title = request.title,
                description = request.description,
                coverImage = request.coverImage,
                medium = request.medium,
                mode = request.mode,
                availability = request.availability,
                tutorId = tutorId,

            };

            _DBcontext.Subjects.Add(subject);
            await _DBcontext.SaveChangesAsync();
            return true;
        }
        public async Task<Subject> GetSubjectById(int id)
        {
            return await _DBcontext.Subjects.FindAsync(id);
        }
        public async Task<Subject> GetSubject(int tutorId, int id)
        {
            var subject = await GetSubjectById(id);
            if (subject == null || subject.tutorId != tutorId)
            {
                return null; // Subject not found or doesn't belong to the tutor
            }
            return subject;
        }

        public async Task<List<Subject>> GetSubjectsByTutorId(int tutorId)
        {
            return await _DBcontext.Subjects.Where(s => s.tutorId == tutorId).ToListAsync();
        }
        //methode for deteting a subject
        public async Task<Subject> DeleteSubject(int id)
        {
            var subject = await _DBcontext.Subjects.FindAsync(id);
            if (subject == null)
            {
                return null;
            }

            _DBcontext.Subjects.Remove(subject);
            await _DBcontext.SaveChangesAsync();
            return subject;
        }
        //update method for updating a subject withautomapper
        public async Task<Subject> UpdateSubject(int id, SubjectRequestDto request)
        {
            var subject = await _DBcontext.Subjects.FindAsync(id);
            if (subject == null)
            {
                return null;
            }

            _mapper.Map(request, subject);
            await _DBcontext.SaveChangesAsync();

            return subject;
        }
        ////get all the subject
        //public async Task<List<Subject>> GetAllSubjects()
        //{
        //    return await _DBcontext.Subjects.ToListAsync();
        //}
        public async Task<List<AllSubject>> GetAllSubjects()
        {
            var subjectsWithRatings = await _DBcontext.Subjects
                .Include(s => s.Tutor)
                .Select(s => new
                {
                    s._id,
                    s.title,
                    s.description,
                    s.coverImage,
                    s.medium,
                    s.mode,
                    s.availability,
                    tutorId = s.Tutor._id,
                    tutorName = s.Tutor.firstName + s.Tutor.lastName, // Fetch the tutor's name
                    ratings = _DBcontext.Reviews
                        .Where(r => r.subjectId == s._id)
                        .Select(r => r.rating)
                        .ToList() // Convert IQueryable<int> to List<int>
                })
                .ToListAsync(); // Fetch data asynchronously into a list

            var subjects = subjectsWithRatings
                .Select(s => new AllSubject
                {
                    _id = s._id,
                    title = s.title,
                    description = s.description,
                    coverImage = s.coverImage,
                    medium = s.medium, // Assuming Medium is a comma-separated string
                    mode = s.mode,
                    availability = s.availability, // Assuming Availability is a comma-separated string
                    tutorId = s.tutorId,
                    tutorName = s.tutorName, // Map the tutor name
                    averageRating = s.ratings.DefaultIfEmpty(0).Average() // Calculate the average rating
                })
                .ToList(); // Convert to a list synchronously

            return subjects;
        }





    }
}
    