using UniTutor.DTO;
using UniTutor.Model;

namespace UniTutor.Interface
{
    public interface ISubject
    {



        public Task<Subject> GetSubjectById(int id);
        public Task<Subject> GetSubject(int tutorId, int id);
        public Task<List<Subject>> GetSubjectsByTutorId(int tutorId);
        public Task<bool> CreateSubject(int tutorId, SubjectRequestDto request);
        public Task<Subject> DeleteSubject(int id);

        public  Task<List<AllSubject>> GetAllSubjects();
        public Task<Subject> UpdateSubject(int id, SubjectRequestDto Request);


    }
}