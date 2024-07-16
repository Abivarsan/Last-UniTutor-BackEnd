using System.ComponentModel.DataAnnotations;

namespace UniTutor.DTO
{
    public class UpdateStudentDto
    {
        public int _id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public string? ProfileUrl { get; set; } // Receive Firebase image URL here
        public string grade { get; set; }
        public string address { get; set; }


    }
}
