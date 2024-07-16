using System.ComponentModel.DataAnnotations;

namespace UniTutor.DTO
{
    public class GetRequest
    {
        public int Id { get; set; }
        public string location { get; set; }
        public string subject { get; set; }
        public string medium { get; set; }
        public string availability { get; set; }

    }
}
