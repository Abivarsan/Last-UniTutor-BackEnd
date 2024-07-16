using UniTutor.DTO;
using UniTutor.Model;

namespace UniTutor.Interface
{
    public interface ITutor
    {
        public bool Delete(int id);
       public Tutor GetById(int id);
        public IEnumerable<Tutor> GetAll();
        public bool login(string email, string password);
        public bool SignUp(Tutor tutor);

        public bool logout();
        
       public Tutor GetTutorByEmail(string email);

        public bool isUser(string email);
        Task<Tutor> GetTutorAsync(int id);
      
        Task UpdateAsync(Tutor tutor);

        Task<Tutor> GetByIdAsync(int id);

        Task<TutorDashboardDetailsDto> GetTutorDashboardDetails(int tutorId);

    





        // public bool createComplaint(Complaint complaint);


    }
}
