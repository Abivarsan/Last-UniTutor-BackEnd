using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using UniTutor.Model;


namespace UniTutor.DTO
{
    public class RequestDto
    {

       
        public int subjectRequestId { get; set; }

        public int subjectId { get; set; }

        public int studentId { get; set; }
        public int tutorId { get; set; }
        public string studentEmail { get; set; } 
        public DateTime timestamp { get; set; }

    }
}
