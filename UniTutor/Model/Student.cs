using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace UniTutor.Model
{
    public class Student
    {

        [Key]
        public int _id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string grade { get; set; }
        public string schoolName { get; set; }
        public string address { get; set; }
       
        public string district { get; set; }
        public string phoneNumber { get; set; }

        [Required]
        [MaxLength(100)]
        public string email { get; set; }

       [Required]
        public string password { get; set; }
        public string? VerificationCode { get; set; }

        public string? ProfileUrl { get; set; }
        public DateTime CreatedAt { get; set;}
        public int?  numberofcomplain {  get; set; } 


        //navigation Property
       
        public  ICollection<Request> Requests { get; set; }
        
        // public virtual ICollection<TodoItem> TodoLists { get; set; }

        public ICollection<TodoItem> TodoItems { get; set; }
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        public bool isSuspended { get; set; }



    }

}
