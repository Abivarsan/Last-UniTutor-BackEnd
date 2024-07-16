namespace UniTutor.DTO
{
    public class CreateReportDto
{
    public string description { get; set; }
    public int reporterId { get; set; }
    public int reportedId { get; set; }
    public string reporterType { get; set; }
    public string reportedType { get; set; }
}
}