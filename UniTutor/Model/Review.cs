using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UniTutor.Model
{
    public class Review
    {
        [Key]
        public int _id { get; set; }

        [ForeignKey("Student")]
        [Required]
        public int studentId { get; set; }

        [Required]
        public int subjectId { get; set; }
        public int rating { get; set; }

        public string feedback { get; set; }

        [Required]
        public DateTime timestamp { get; set; }
        public virtual Student Student { get; set; }
        public virtual Subject Subject { get; set; }

    }
}
