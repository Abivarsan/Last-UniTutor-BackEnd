namespace UniTutor.DTO
{
    public class CreateCheckoutSessionDto
    {
        public int Coins { get; set; }
        public string Description { get; set; }
        public string? PaymentMethod { get; set; }
    }
}
