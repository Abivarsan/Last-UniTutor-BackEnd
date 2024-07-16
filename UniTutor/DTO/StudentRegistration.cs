using System.ComponentModel.DataAnnotations;

namespace UniTutor.DTO
{
    public class StudentRegistration
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string grade { get; set; }
        public string schoolName { get; set; }
        public string address { get; set; }
        public string district { get; set; }
        public string phoneNumber { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
