using UniTutor.Model;

namespace UniTutor.Interface
{
    public interface IUser
    {
        Task<Tutor> GetTutorByIdAsync(int id);
        Task<Student> GetStudentByIdAsync(int id);
        public Task DeleteTutorAsync(int tutorId);
        public  Task DeleteStudentAsync(int studentId);
    }
}
