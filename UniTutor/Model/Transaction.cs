using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniTutor.Model
{
    public class Transaction
    {
        
        public int Id { get; set; }
        public DateTime timestamp { get; set; }
        public string Description { get; set; }
        public int Coins { get; set; }

        [ForeignKey("Tutor")]
        public int tutorId { get; set; }
        public Tutor Tutor { get; set; }
        public string? StripeSessionId { get; set; }
        
    }
}
