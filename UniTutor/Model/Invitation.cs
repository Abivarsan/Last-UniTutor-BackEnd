namespace UniTutor.Model
{
    public class Invitation
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string VerificationCode { get; set; }
        public int InvitedById { get; set; }
        public bool IsVerified { get; set; }
    }
}
