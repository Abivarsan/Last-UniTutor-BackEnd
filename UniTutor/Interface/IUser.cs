using UniTutor.Model;

namespace UniTutor.Interface
{
    public interface IUser
    {
        Task<Tutor> GetTutorByIdAsync(int id);
        Task<Student> GetStudentByIdAsync(int id);
        Task DeleteTutorAsync(Tutor tutor);
        public  Task DeleteStudentAsync(int studentId);
    }
}
