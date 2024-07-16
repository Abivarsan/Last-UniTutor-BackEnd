namespace UniTutor.DTO
{
    public class WeeklyDataDto
    {
        public string Day { get; set; }
        public int Count { get; set; }

        public bool Verified { get; set; } = false;
    }

}
