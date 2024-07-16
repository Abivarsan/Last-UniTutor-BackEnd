using AutoMapper;
using Azure.Core;
using UniTutor.DTO;
using UniTutor.Model;

namespace UniTutor.Interface
{
    public interface IStudent
    {
        public bool SignUp(Student student);
        public bool Login(string email, string password);
        public Student GetByMail(string Email);
        // for delete 
        bool Delete(int id);
        public Student GetById(int id);
        public bool SignOut();
       // public bool CreateRequest(Model.Request request);
       // public bool DeleteRequest(Model.Request request);
       
        public Task<bool> Update(Student student);
        Task DeleteStudentAsync(int id);
        

        //update student using studentUpdateDto
        Task UpdateAsync(Student student);

        Task<Student> GetByIdAsync(int id);

        Task<StudentDashboardDeatilsDto> GetStudentDashboardDetails(int studentId);

        


    }
}
