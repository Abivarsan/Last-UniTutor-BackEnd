    using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime;

namespace UniTutor.Model
{
    public class Tutor
    {
        [Key]
        public int _id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string password { get; set; }
        public string occupation { get; set; }
        public string address { get; set; }
        public string district { get; set; }
        public string phoneNumber { get; set; }
        [Required]
        [MaxLength(100)]
        public string universityMail { get; set; }
        public string qualifications { get; set; }
        public string cv { get; set; }

        public string universityID { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ProfileUrl { get; set; }

        public bool Verified { get; set; }=false;
        public string? VerificationCode { get; set; }

        public ICollection<Subject> Subjects { get; set; }
        public ICollection<Transaction> Transactions { get; set; }


        public  ICollection<Request> Requests { get; set; }
        // public virtual ICollection<TodoItem> TodoLists { get; set; }

        public ICollection<TodoItem> TodoItems { get; set; }

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        public int Coins { get; set; }

        public bool   isSuspended {  get; set; } = false;




     




    }
}
