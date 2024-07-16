using System.Security.Claims;
using UniTutor.DTO;
using UniTutor.Model;

namespace UniTutor.Interface
{
    public interface IAdmin
    {


        //public ClaimsPrincipal validateToken(string token);

        //public bool IsAdmin(Admin admin);

        //public Admin GetAdminByEmail(string Email);
        //public Admin GetAdminById(int Id);

        //public bool Logout();
        //bool Login(string email, string password);
        //bool CreateAdmin(Admin admin);

        //public IEnumerable<Student> GetAllStudent();
        //public IEnumerable<Tutor> GetAllTutor();
        //public IEnumerable<Tutor> GetTutorVerification();
        //Tutor  GetTutorById(int id);
        //public  Task AcceptTutor(int id);
        //public Task RejectTutor(int id);

        //Student GetStudentById(int id);

        //Task<bool> AdminLoginAsync(AdminLoginDto LoginDto);
        //Task SendEmailAsync(SendReportEmailDto emailDto);

        bool CreateAdmin(Admin admin);
        bool Login(string email, string password);
        Task<bool> AdminLoginAsync(AdminLoginDto loginDto);
        bool IsAdmin(Admin admin);
        ClaimsPrincipal ValidateToken(string token);
        Admin GetAdminByEmail(string email);
        Admin GetAdminById(int id);
        IEnumerable<Student> GetAllStudent();
        IEnumerable<Tutor> GetAllTutor();
        Tutor GetTutorById(int id);
        Task AcceptTutor(int id);
        Task RejectTutor(int id);
        bool Logout();
        Student GetStudentById(int id);
        IEnumerable<Tutor> GetTutorVerification();
        Task SendReportEmailAsync(SendReportEmailDto emailDto);





    }
}
