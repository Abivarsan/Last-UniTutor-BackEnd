
﻿using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;

namespace UniTutor.Model
{


    public class Comment
    {
        [Key]
        public int _id { get; set; }
        public string userType { get; set; }
        public string commentText { get; set; }
        public DateTime timestamp { get; set; }

        // Foreign key to Student or Tutor
        public int? stuId { get; set; }
        public int? tutId { get; set; }

        public Student Student { get; set; }
        public Tutor Tutor { get; set; }

        [NotMapped]  
        public string? fullName => userType == "Student" ? $"{Student.firstName} {Student.lastName}" : $"{Tutor.firstName} {Tutor.lastName}";
        public string? profileUrl => userType == "Student" ? $"{Student.ProfileUrl}" : $"{Tutor.ProfileUrl} ";
    }



}
