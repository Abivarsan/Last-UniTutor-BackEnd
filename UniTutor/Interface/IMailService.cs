namespace UniTutor.Interface
{
    public interface IMailService
    {
        void SendEmail(string toEmail, string subject, string body);
    }
}
