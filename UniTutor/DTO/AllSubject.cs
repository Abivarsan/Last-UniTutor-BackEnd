namespace UniTutor.DTO
{
    public class AllSubject
    {
        public int _id { get; set; }

        public string title { get; set; }
        public string description { get; set; }
        public string coverImage { get; set; }
        public string[] medium { get; set; }
        public string mode { get; set; }
        public string[] availability { get; set; }
        public string tutorName { get; set; }
        public double averageRating { get; set; } = 0;

        public int tutorId { get; set; }

    }
}
