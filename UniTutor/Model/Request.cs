using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace UniTutor.Model
{
    public class Request
    {
        [Key]
        public int _id { get; set; }

        [Required]
        public int subjectId { get; set; }
        public  Subject Subject { get; set; }

        [ForeignKey("Student")]
        [Required]
        public int studentId { get; set; }
        public  Student Student { get; set; }

        [ForeignKey("Tutor")]
        [Required]
        public int tutorId { get; set; }
        public  Tutor Tutor { get; set; }

        [Required]
        [EmailAddress]
        public string studentEmail { get; set; }

        [Required]
        [StringLength(20)]
        public string status { get; set; } = "PENDING";

        [Required]
        public DateTime timestamp { get; set; }

        
       
       
     

        public bool IsAccepted { get; set; }
        public bool IsRejected { get; set; }


    }
}