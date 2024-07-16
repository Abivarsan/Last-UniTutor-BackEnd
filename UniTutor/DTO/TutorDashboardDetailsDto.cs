using System.ComponentModel.DataAnnotations;

namespace UniTutor.DTO
{
    public class TutorDashboardDetailsDto
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string occupation { get; set; }
        public string address { get; set; }
        public string district { get; set; }
        public string phoneNumber { get; set; }
        public string universityMail { get; set; }
        public string qualifications { get; set; }
        public string? ProfileUrl { get; set; }
    }
}
