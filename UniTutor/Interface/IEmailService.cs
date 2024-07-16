namespace UniTutor.Interface
{
    public interface IEmailService
    {  
        Task SendVerificationCodeAsync(string email, string verificationCode);
        public  Task SendEmailAsync(string recipient, string subject, string body);

        //Task SendSmsAsync(string phoneNumber, string message);
    }
}
