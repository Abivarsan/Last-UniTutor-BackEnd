using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UniTutor.Model
{
    public class TodoItem
    {
    
        public bool isCompleted { get; set; } = false;
        public virtual Student Student { get; set; }

        [ForeignKey("Student")]

        public int? studentId { get; set; }
       
        public string text { get; set; }

        public virtual Tutor Tutor { get; set; }

        [ForeignKey("Tutor")]

        public int? tutorId { get; set; }
        [Key]
        public int _id { get; set; }

    }
}
