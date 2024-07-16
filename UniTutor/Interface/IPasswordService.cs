namespace UniTutor.Interface
{
    public interface IPasswordService
    {
        Task SendVerificationCodeAsync(string email);
        //Task<bool> ResetPasswordAsync(string email, string verificationCode, string newPassword);
        public Task<bool> VerifyOtpAsync(string verificationCode);
        public  Task<bool> ResetPasswordAsync(string newPassword);
    }
}
