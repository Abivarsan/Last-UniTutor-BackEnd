using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore;
using System.Threading.Tasks;
using UniTutor.DataBase;
using UniTutor.Model;
using UniTutor.Interface;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using UniTutor.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using UniTutor.Services;
using UniTutor.DTO;

using AutoMapper;

namespace UniTutor.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        IStudent _student;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper; 
        private readonly IEmailService _emailService;
       


        public StudentController(IStudent student,IConfiguration config,IMapper mapper,IEmailService emailService)
        {
            _config = config;
            _student = student;
            _mapper = mapper;
            _emailService = emailService;
            
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateAccountAsync([FromBody] StudentRegistration studentDto)
        {
            if (ModelState.IsValid)
            {
                // Set CreatedAt to local time
                TimeZoneInfo localZone = TimeZoneInfo.FindSystemTimeZoneById("Sri Lanka Standard Time"); // Change to your local time zone
                DateTime localDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, localZone);

                var student = _mapper.Map<Student>(studentDto);
                PasswordHash ph = new PasswordHash();
                student.password = ph.HashPassword(studentDto.password); // Hash the password
                student.CreatedAt = localDateTime;

                var result = _student.SignUp(student);
                if (result)
                {
                    Console.WriteLine("registration success");
                    //sending email 

                    var emailSubject = "Welcome to UniTutor!";

                    var emailMessage = $@"
                        <html>
                        <head>
                        <style>
                            body {{
                                font-family: Arial, sans-serif;
                                line-height: 1.6;
                            }}
                            .email-container {{
                                width: 100%;
                                padding: 20px;
                                background-color: lightblue; /* Changed to blue */
                            }}
                            .email-header {{
                                background-color: #024A7B; /* Changed to blue */
                                color: white;
                                padding: 10px 20px;
                                text-align: center;
                            }}
                            .email-content {{
                                background-color: white;
                                padding: 20px;
                                margin-top: 20px;
                            }}
                            .email-footer {{
                                margin-top: 20px;
                                text-align: center;
                                font-size: 0.9em;
                                color: #777;
                            }}
                         </style>
                         </head>
                         <body>
                         <div class='email-container'>
                         <div class='email-header'>
                         <h1>UniTutor</h1>
                         </div>
                         <div class='email-content'>
                            <p>Dear {student.firstName},</p>
                            <p>Welcome to UniTutor! We are glad to have you on board.</p>
                            <p>We look forward to supporting you in your academic journey. Our platform is designed to connect you with the best tutors to succeed in your studies.</p>
                            <p>If you have any questions or need assistance, please feel free to reach out to our support team at any time.</p>
                            <p>Best regards,<br>UniTutor Team</p>
                         </div>
                         <div class='email-footer'>
                            <p>This is an automated message, please do not reply directly to this email.</p>
                            <p>If you have any questions, feel free to contact our support team at support@unitutor.com.</p>
                         </div>
                         </div>
                         </body>
                         </html>";

                    await _emailService.SendEmailAsync(student.email, emailSubject, emailMessage);
                    return CreatedAtAction(nameof(GetAccountById), new { id = student._id }, student);
                }
                else
                {
                    Console.WriteLine("registration failed");
                    return BadRequest(result);
                }
            }
            else
            {
                return BadRequest("ModelError");
            }
        }
        [HttpGet("details/{id}")]
        public IActionResult GetAccountById(int id)
        {
            var student = _student.GetById(id);
            if (student == null)
            {
                return NotFound();
            }
            return Ok(student);
        }


        [HttpPost("login")]
        public IActionResult Login([FromBody] Loginrequest studentLogin)
        {
            var email = studentLogin.email;
            var password = studentLogin.password;

            var result = _student.Login(email, password);
            if (!result)
            { 
                return Unauthorized("Invalid email or password");
            }
            var loggedInStudent = _student.GetByMail(email);

            if (loggedInStudent.isSuspended)
            {
                return Unauthorized("Account is suspended. Please contact the administrator.");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                 new Claim(ClaimTypes.Name, email),  // Email claim
                 new Claim(ClaimTypes.NameIdentifier, loggedInStudent._id.ToString()),  // Student ID claim
                 new Claim(ClaimTypes.GivenName, loggedInStudent.firstName),  // Student name claim
                 new Claim(ClaimTypes.Role, "Student")
                    }),
                    Expires = DateTime.UtcNow.AddDays(30),
                    SigningCredentials = credentials
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);

                return Ok(new { token = tokenHandler.WriteToken(token), Id = loggedInStudent._id });
        }
            

            
        

        [HttpPut("ProfileUpdate{id}")]
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] UpdateStudentDto updateStudentDto)
        {
            if (id != updateStudentDto._id)
            {
                return BadRequest();
            }

            var student = await _student.GetByIdAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            student.firstName = updateStudentDto.firstName;
            student.lastName = updateStudentDto.lastName;
            student.email = updateStudentDto.email;
            student.phoneNumber = updateStudentDto.phoneNumber;
            student.grade = updateStudentDto.grade;
            student.address = updateStudentDto.address;

            if (!string.IsNullOrEmpty(updateStudentDto.ProfileUrl))
            {
                student.ProfileUrl = updateStudentDto.ProfileUrl;
            }

            await _student.UpdateAsync(student);

            return NoContent();
        }

        [HttpGet("{studentId}")]
        public async Task<IActionResult> GetStudentDashboardDetails(int studentId)
        {
            var studentDetails = await _student.GetStudentDashboardDetails(studentId);
            if (studentDetails == null)
            {
                return NotFound();
            }
            return Ok(studentDetails);
        }
        //[HttpPost("requesttutor")]
        //public IActionResult requesttutor([FromBody] Request request)
        //{
        //    var result = _student.CreateRequest(request);
        //    if (result)
        //    {
        //        return Ok(result);
        //    }
        //    else
        //    {
        //        return BadRequest();
        //    }
        //}

        //[HttpDelete("deleterequest")]
        //public IActionResult deleterequest([FromBody] Request request)
        //{
        //    var result = _student.DeleteRequest(request);
        //    if (result)
        //    {
        //        return Ok(result);
        //    }
        //    else
        //    {
        //        return BadRequest();
        //    }
        //}




    }
}
