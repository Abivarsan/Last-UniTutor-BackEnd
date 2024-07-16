using System.ComponentModel.DataAnnotations;

namespace UniTutor.Model
{
    public class Admin
    {

        [Key]
        public int _id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Email { get; set; }


        public string Name { get; set; }

        [Required]
        public string password { get; set; }
    }
}

