using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniTutor.Model
{
    public class Report
    {
        [Key]
        public int _id { get; set; }
        public string description { get; set; }
        public DateTime date { get; set; }
        public int reporterId { get; set; }    // e.g., "studentId" or "tutorId"
        public int reportedId { get; set; }         // e.g., "studentId" or "tutorId"
        public string reporterType { get; set; } // e.g., "Student" or "Tutor"
        public string reportedType { get; set; }    // e.g., "Student" or "Tutor"

    }
}