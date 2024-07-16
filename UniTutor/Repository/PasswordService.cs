using UniTutor.Interface;
using UniTutor.Model;

namespace UniTutor.Repository
{
    public class PasswordService : IPasswordService
    {
        private static string _tempEmailForVerification; // Temporary storage for email
        private readonly IStudent _student;
        private readonly ITutor _tutor;
        private readonly IEmailService _emailService;

        public PasswordService(IStudent student, ITutor tutor, IEmailService emailService)
        {
            _student = student;
            _tutor = tutor;
            _emailService = emailService;
        }

        public async Task SendVerificationCodeAsync(string email)
        {
            _tempEmailForVerification = email;
            var student = _student.GetByMail(email);
            var tutor = _tutor.GetTutorByEmail(email);

            if (student != null)
            {
                // Generate verification code
                var verificationCode = Guid.NewGuid().ToString("N").Substring(0, 6);
                student.VerificationCode = verificationCode;
                await _student.Update(student);
                await _emailService.SendVerificationCodeAsync(student.email, verificationCode);
            }
            else if (tutor != null)
            {
                // Generate verification code
                var verificationCode = Guid.NewGuid().ToString("N").Substring(0, 6);
                tutor.VerificationCode = verificationCode;
                await _tutor.UpdateAsync(tutor);
                await _emailService.SendVerificationCodeAsync(tutor.universityMail, verificationCode);
            }
            else
            {
                throw new Exception("Email not found");
            }
        }

        //public async Task<bool> ResetPasswordAsync(string email, string verificationCode, string newPassword)
        //{
        //    var student = _student.GetByMail(email);
        //    var tutor = _tutor.GetTutorByEmail(email);

        //    if (student != null && student.VerificationCode == verificationCode)
        //    {
        //        // Update the password
        //        PasswordHash ph = new PasswordHash();
        //        student.password = ph.HashPassword(newPassword);
        //        student.VerificationCode = null; // Clear the verification code after successful reset
        //        await _student.Update(student);
        //        return true;
        //    }
        //    else if (tutor != null && tutor.VerificationCode == verificationCode)
        //    {
        //        // Update the password
        //        PasswordHash ph = new PasswordHash();
        //        tutor.password = ph.HashPassword(newPassword);
        //        tutor.VerificationCode = null; // Clear the verification code after successful reset
        //        await _tutor.UpdateTutorAsync(tutor);
        //        return true;
        //    }

        //    return false; // If no user matched or verification code didn't match
        //}
        public async Task<bool> VerifyOtpAsync(string verificationCode)
        {
            var email = _tempEmailForVerification; // Use the stored email
            var student = _student.GetByMail(email);
            var tutor = _tutor.GetTutorByEmail(email);

            if (student != null && student.VerificationCode == verificationCode)
            {
                return true;
            }
            else if (tutor != null && tutor.VerificationCode == verificationCode)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> ResetPasswordAsync(string newPassword)
        {
            var email = _tempEmailForVerification; // Use the stored email
            var student =  _student.GetByMail(email);
            var tutor = _tutor.GetTutorByEmail(email);

            if (student != null)
            {
                PasswordHash ph = new PasswordHash();
                student.password = ph.HashPassword(newPassword);
                student.VerificationCode = null; // Clear verification code after reset
                await _student.Update(student);
                return true;
            }
            else if (tutor != null)
            {
                PasswordHash ph = new PasswordHash();
                tutor.password = ph.HashPassword(newPassword);
                tutor.VerificationCode = null; // Clear verification code after reset
                await _tutor.UpdateAsync(tutor);
                return true;
            }

            return false; // If no user matched
        }
    } 
}
