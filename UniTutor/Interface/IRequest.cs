using UniTutor.DTO;
using UniTutor.Model;

namespace UniTutor.Interface
{
    public interface IRequest
    {
        Task<IEnumerable<Request>> GetAll();
        Task<Request> GetById(int id);
        Task<IEnumerable<Request>> GetByStudentId(int studentId);
        Task<IEnumerable<Request>> GetByTutorId(int tutorId);
        Task<Request> Create(RequestDto request);
        
        Task<Request> Delete(int id);

        
        public  Task<Request> UpdateRequestStatus(int id, string status);
        //get all requests by tutor id
       
        Task<IEnumerable<Request>> GetAcceptedRequestsByTutorId(int tutorId);
        public Task<IEnumerable<Request>> GetAcceptedRequestsByStudentId(int studentId);
        public  Task<IEnumerable<Request>> GetAllRequestsByStudentId(int studentId);

        Task<int> GetMySubjectsCount(int studentId);
        Task<int> GetAcceptedRequestsCount(int studentId);
        Task<int> GetRejectedRequestsCount(int studentId);


        public  Task<int> GetMySubjectsCountTutor(int tutorId);
        public  Task<int> GetMyStudentCount(int tutorId);
        public  Task<int> GetAllRequestscount(int tutorId);
       
    }
}

