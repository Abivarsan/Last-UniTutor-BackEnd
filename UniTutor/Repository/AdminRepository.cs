
//using Braintree;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
//using System.Net.Mail;
//using System.Net;
//using System.Security.Claims;
//using System.Text;
//using UniTutor.DataBase;
//using UniTutor.DTO;
//using UniTutor.Interface;
//using UniTutor.Model;

//namespace UniTutor.Repository
//{
//    public class AdminRepository:IAdmin
//    {
//        private ApplicationDBContext _DBcontext;
//        private readonly IConfiguration _config;


//        public AdminRepository(ApplicationDBContext DBcontext, IConfiguration config)
//        {
//            _DBcontext = DBcontext;
//            _config = config;

//        }

//        public bool CreateAdmin(Admin admin)
//        {
//            try
//            {
//                PasswordHash ph = new PasswordHash();
//                admin.password = ph.HashPassword(admin.password);
//                _DBcontext.Admin.Add(admin);
//                _DBcontext.SaveChanges(true);
//                return true;
//            }
//            catch (Exception ex)
//            {
//                return false;
//            }
//        }
//        public bool Login(string email, string password)
//        {
//            var admin = _DBcontext.Admin.FirstOrDefault(a => a.Email == email);

//            if (admin == null)
//            {
//                return false;
//            }

//            PasswordHash ph = new PasswordHash();

//            bool isValidPassword = ph.VerifyPassword(password, admin.password);

//            if (isValidPassword)
//            {
//                return true;
//            }
//            else
//            {
//                return false;
//            }
//        }
//        public async Task<bool> AdminLoginAsync(AdminLoginDto loginDto)
//        {

//            var admin = _DBcontext.Admin.FirstOrDefault(a => a.Email == loginDto.Username);
//            if (admin == null)
//            {
//                return false;
//            }

//            PasswordHash ph = new PasswordHash();

//            bool isValidPassword = ph.VerifyPassword(loginDto.Password, admin.password);

//            if (isValidPassword)
//            {
//                return true;
//            }
//            else
//            {
//                return false;
//            }
//        }

//        public bool IsAdmin(Admin admin)
//        {
//            return _DBcontext.Admin.Any(a => a.Email == admin.Email);
//        }
//        public ClaimsPrincipal validateToken(string token)
//        {
//            try
//            {
//                var tokenHandler = new JwtSecurityTokenHandler();
//                var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);
//                tokenHandler.ValidateToken(token, new TokenValidationParameters
//                {
//                    ValidateIssuerSigningKey = true,
//                    IssuerSigningKey = new SymmetricSecurityKey(key),
//                    ValidateIssuer = false,
//                    ValidateAudience = false,
//                    ClockSkew = TimeSpan.Zero
//                }, out SecurityToken validatedToken);

//                if (validatedToken != null)
//                {
//                    return new ClaimsPrincipal(new ClaimsIdentity(((JwtSecurityToken)validatedToken).Claims));
//                }

//                return null;
//            }
//            catch
//            {
//                return null;
//            }
//        }
//        public Admin GetAdminByEmail(string Email)
//        {
//            return _DBcontext.Admin.FirstOrDefault(x => x.Email == Email);
//        }

//        public Admin GetAdminById(int Id)
//        {
//            return _DBcontext.Admin.FirstOrDefault(x => x._id == Id);
//        }
//        public IEnumerable<Student> GetAllStudent()
//        {
//            return _DBcontext.Students.ToList();
//        }
//        public IEnumerable<Tutor> GetAllTutor()
//        {
//            return _DBcontext.Tutors.ToList();
//        }
//        public Tutor GetTutorById(int id)
//        {
//            // Example implementation, ensure it handles null case
//            return  _DBcontext.Tutors.Find(id);

//        }

//        public async Task AcceptTutor(int id)
//        {
//            // Implement logic to accept a tutor
//            var tutor = await _DBcontext.Tutors.FindAsync(id);
//            if (tutor != null)
//            {
//                tutor.Verified = true; // Example: Update tutor status
//                await _DBcontext.SaveChangesAsync();
//            }
//        }

//        public async Task RejectTutor(int id)
//        {
//            // Implement logic to reject a tutor
//            var tutor = await _DBcontext.Tutors.FindAsync(id);
//            if (tutor != null)
//            {
//                tutor.Verified = false; // Example: Update tutor status
//                _DBcontext.Tutors.Remove(tutor);
//                await _DBcontext.SaveChangesAsync();
//            }
//        }
//        public bool Logout()
//        {
//            throw new NotImplementedException();
//        }
//        public Student GetStudentById(int id)
//        {
//            return _DBcontext.Students.Find(id);
//        }
//        //tuter verification
//        public IEnumerable<Tutor> GetTutorVerification()
//        {
//            return _DBcontext.Tutors.Where(t => t.Verified == false).ToList();
//        }

//        public async Task SendEmailAsync(SendReportEmailDto emailDto)
//        {
//            var smtpClient = new SmtpClient(_config["Smtp:Host"])
//            {
//                Port = int.Parse(_config["Smtp:Port"]),
//                Credentials = new NetworkCredential(_config["Smtp:Username"], _config["Smtp:Password"]),
//                EnableSsl = true,
//            };

//            var mailMessage = new MailMessage
//            {
//                From = new MailAddress(_config["Smtp:Username"]),
//                Subject = "Profile Deletion Notification",
//                Body = emailDto.Message,
//                IsBodyHtml = true,
//            };

//            mailMessage.To.Add(emailDto.To);

//            await smtpClient.SendMailAsync(mailMessage);
//        }
//    }
//}

using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using UniTutor.DataBase;
using UniTutor.DTO;
using UniTutor.Interface;
using UniTutor.Model;

namespace UniTutor.Repository
{
    public class AdminRepository : IAdmin
    {
        private readonly ApplicationDBContext _DBcontext;
        private readonly IConfiguration _config;

        public AdminRepository(ApplicationDBContext DBcontext, IConfiguration config)
        {
            _DBcontext = DBcontext;
            _config = config;
        }

        public bool CreateAdmin(Admin admin)
        {
            try
            {
                PasswordHash ph = new PasswordHash();
                admin.password = ph.HashPassword(admin.password);
                _DBcontext.Admin.Add(admin);
                _DBcontext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here
                return false;
            }
        }

        public bool Login(string email, string password)
        {
            var admin = _DBcontext.Admin.FirstOrDefault(a => a.Email == email);

            if (admin == null)
            {
                return false;
            }

            PasswordHash ph = new PasswordHash();
            return ph.VerifyPassword(password, admin.password);
        }

        public async Task<bool> AdminLoginAsync(AdminLoginDto loginDto)
        {
            var admin = _DBcontext.Admin.FirstOrDefault(a => a.Email == loginDto.Username);
            if (admin == null)
            {
                return false;
            }

            PasswordHash ph = new PasswordHash();
            return ph.VerifyPassword(loginDto.Password, admin.password);
        }

        public bool IsAdmin(Admin admin)
        {
            return _DBcontext.Admin.Any(a => a.Email == admin.Email);
        }

        public ClaimsPrincipal ValidateToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return validatedToken != null
                    ? new ClaimsPrincipal(new ClaimsIdentity(((JwtSecurityToken)validatedToken).Claims))
                    : null;
            }
            catch
            {
                return null;
            }
        }

        public Admin GetAdminByEmail(string email)
        {
            return _DBcontext.Admin.FirstOrDefault(x => x.Email == email);
        }

        public Admin GetAdminById(int id)
        {
            return _DBcontext.Admin.FirstOrDefault(x => x._id == id);
        }

        public IEnumerable<Student> GetAllStudent()
        {
            return _DBcontext.Students.ToList();
        }

        public IEnumerable<Tutor> GetAllTutor()
        {
            return _DBcontext.Tutors.ToList();
        }

        public Tutor GetTutorById(int id)
        {
            return _DBcontext.Tutors.Find(id);
        }

        public async Task AcceptTutor(int id)
        {
            var tutor = await _DBcontext.Tutors.FindAsync(id);
            if (tutor != null)
            {
                tutor.Verified = true;
                await _DBcontext.SaveChangesAsync();
            }
        }

        public async Task RejectTutor(int id)
        {
            var tutor = await _DBcontext.Tutors.FindAsync(id);
            if (tutor != null)
            {
                tutor.Verified = false;
                _DBcontext.Tutors.Remove(tutor);
                await _DBcontext.SaveChangesAsync();
            }
        }

        public bool Logout()
        {
            throw new NotImplementedException();
        }

        public Student GetStudentById(int id)
        {
            return _DBcontext.Students.Find(id);
        }

        public IEnumerable<Tutor> GetTutorVerification()
        {
            return _DBcontext.Tutors.Where(t => !t.Verified).ToList();
        }

        public async Task SendReportEmailAsync(SendReportEmailDto emailDto)
        {
            var smtpHost = _config["Smtp:Host"];
            var smtpPort = _config["Smtp:Port"];
            var smtpUsername = _config["Smtp:Username"];
            var smtpPassword = _config["Smtp:Password"];

            // Log the values to ensure they are not null or empty (use your preferred logging mechanism)
            Console.WriteLine($"SMTP Host: {smtpHost}");
            Console.WriteLine($"SMTP Port: {smtpPort}");
            Console.WriteLine($"SMTP Username: {smtpUsername}");
            Console.WriteLine($"SMTP Password: {smtpPassword}");

            if (string.IsNullOrWhiteSpace(smtpHost) || string.IsNullOrWhiteSpace(smtpPort) || string.IsNullOrWhiteSpace(smtpUsername) || string.IsNullOrWhiteSpace(smtpPassword))
            {
                throw new ArgumentNullException("SMTP configuration values cannot be null or empty.");
            }

            var smtpClient = new SmtpClient(smtpHost)
            {
                Port = int.Parse(smtpPort),
                Credentials = new NetworkCredential(smtpUsername, smtpPassword),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(smtpUsername),
                Subject = "Profile Deletion Notification",
                Body = emailDto.Message,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(emailDto.To);

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
