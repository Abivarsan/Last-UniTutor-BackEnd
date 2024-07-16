namespace UniTutor.DTO
{
    public class LastJoined
    {
        public int _id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string ProfileUrl { get; set; }
        public string Type { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
