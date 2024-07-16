//using Microsoft.Extensions.Configuration;
//using System.Threading.Tasks;
//using UniTutor.Interface;

//namespace UniTutor.Services
//{
//    public class TutorService
//    {
//        private readonly IEmailService _emailService;
//        private readonly string _adminPhoneNumber;

//        public TutorService(IEmailService emailService, IConfiguration configuration)
//        {
//            _emailService = emailService;
//            _adminPhoneNumber = configuration["AdminPhoneNumber"];
//        }

//        public async Task RequestToJoinAsync(string tutorName)
//        {
//            string message = $"Tutor {tutorName} has requested to join the platform.";
//            await _emailService.SendSmsAsync(_adminPhoneNumber, message);
//        }
//    }
//}
