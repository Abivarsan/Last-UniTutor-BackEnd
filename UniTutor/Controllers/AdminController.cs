﻿ using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UniTutor.DTO;
using UniTutor.Interface;
using UniTutor.Model;
using UniTutor.Repository;

namespace UniTutor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdmin _admin;
        private IConfiguration _config;
        private readonly IStudent _student;
        private readonly ITutor _tutor;
        private readonly IEmailService _emailService;
        private readonly IUser _userRepository;




        public AdminController(IAdmin adminRepository, IConfiguration config, IStudent student, ITutor tutor, IEmailService emailService, IUser userRepository)
        {
            _admin = adminRepository;
            _config = config;
            _student = student;
            _tutor = tutor;
            _emailService = emailService;
            _userRepository = userRepository;
        }


        [HttpPost("create")]
        public IActionResult CreateAdmin([FromBody] Admin admin)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                var isAdmin = _admin.IsAdmin(admin);
                if (!isAdmin)
                {
                    var result = _admin.CreateAdmin(admin);
                    if (result)
                    {
                        return Ok();
                    }
                    else
                    {
                        return BadRequest(ModelState);
                    }
                }
                else
                {
                    return NotFound();
                }
            }
        }

        [HttpPost("adminLogin")]
        public IActionResult AdminLogin([FromBody] LoginRequest adminLogin)
        {
            var result = _admin.Login(adminLogin.Email, adminLogin.Password);
            if (result)
            {
                // Retrieve admin details from the database
                var loggedInAdmin = _admin.GetAdminByEmail(adminLogin.Email);

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                new Claim(ClaimTypes.Name, adminLogin.Email),  // Email claim
                new Claim(ClaimTypes.NameIdentifier, loggedInAdmin._id.ToString()),  // Admin ID claim
                new Claim(ClaimTypes.GivenName, loggedInAdmin.Name),  // Admin name claim
                new Claim(ClaimTypes.Role, "Student")

                    }),
                    Expires = DateTime.UtcNow.AddDays(30),
                    SigningCredentials = credentials
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);

                return Ok(new { token = tokenHandler.WriteToken(token), Id = loggedInAdmin._id });
            }
            else
            {
                return Unauthorized("Invalid email or password");
            }
        }
        [HttpPost("relogin")]
        public async Task<IActionResult> AdminLogin([FromBody] AdminLoginDto loginDto)
        {
            var result =  _admin.Login(loginDto.Username, loginDto.Password);

            if (result)
            {
                return Ok(new { success = true });
            }

            return Unauthorized(new { success = false, message = "Invalid login credentials" });
        }
        [HttpPost("Report/send-email")]
        public async Task<IActionResult> SendReportEmail([FromBody] SendReportEmailDto emailDto)
        {
            await _admin.SendReportEmailAsync(emailDto);
            return Ok(new { success = true });
        }


        //[HttpGet("isAuthenticated")]
        //public IActionResult isAthenticated([FromQuery(Name = "token")] string token)
        //{
        //    var validatedToken = _admin.validateToken(token);
        //    if (validatedToken != null)
        //    {
        //        return Ok(new { authenticated = true });
        //    }
        //    else
        //    {
        //        return Unauthorized(new { authenticated = false });
        //    }
        //}


        //[HttpDelete("Tutordetails/{id}")]
        //public async Task<IActionResult> DeleteTutor(int id)
        //{
        //    var tutor = await _userRepository.GetTutorByIdAsync(id);
        //    if (tutor == null)
        //    {
        //        return NotFound(new { success = false, message = "Tutor not found" });
        //    }

        //    await _userRepository.DeleteTutorAsync(tutor);
        //    return Ok(new { success = true });
        //}

        [HttpDelete("Tutordetails/{id}")]
        public async Task<IActionResult> DeleteTutor(int id)
        {
            try
            {
                await _userRepository.DeleteTutorAsync(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return Ok(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, new { message = "An error occurred while processing your request.", details = ex.Message });
            }
        }


        [HttpDelete("Studentdetails/{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            try
            {
                await _userRepository.DeleteStudentAsync(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return Ok(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, new { message = "An error occurred while processing your request.", details = ex.Message });
            }
        }


        [HttpGet("AllStudents")]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            var students = _admin.GetAllStudent();
            if (students != null)
            {
                return Ok(students);
            }
            else
            {
                return BadRequest("There is no student");
            }
        }
        [HttpGet("AllTutors")]
        public async Task<ActionResult<IEnumerable<Tutor>>> GetTutors()
        {
            var tutors = _admin.GetAllTutor();
            if (tutors != null)
            {
                return Ok(tutors);
            }
            else
            {
                return BadRequest("There is no student");
            }
        }
        //get verified = false tutor list
        [HttpGet("TutorVerification")]
        public async Task<ActionResult<IEnumerable<Tutor>>> GetTutorVerification()
        {
            var tutors = _admin.GetTutorVerification();
            if (tutors != null)
            {
                return Ok(tutors);
            }
            else
            {
                return BadRequest("There is no tutor");
            }
        }
       




        [HttpPost("accepttutor/{id}")]
        public async Task<IActionResult> AcceptTutor(int id)
        {
            var tutor = _admin.GetTutorById(id);
            if (tutor == null)
            {
                return NotFound();
            }

            // Perform acceptance logic
            await _admin.AcceptTutor(id);

            // Send verification email
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
                            <p>Dear {tutor.firstName},</p>
                            <p>We are pleased to inform you that your submitted ID and details have been verified, and your tutor account has been accepted.</p>
                            <p>You can now log in to your account using your password and start posting your subjects on our platform.</p>
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
            await _emailService.SendEmailAsync(tutor.universityMail, emailSubject, emailMessage);


            return Ok();
        }
        [HttpDelete("rejecttutor/{id}")]
        public async Task<IActionResult> RejectTutor(int id)
        {
            var tutor = _admin.GetTutorById(id);
            if (tutor == null)
            {
                return NotFound();
            }

            // Perform rejection logic
            await _admin.RejectTutor(id);

            // Send rejection email
            var emailSubject = "UniTutor Registration Request Rejected";

            var emailMessage = $@"
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
                         <p>Dear {tutor.firstName},</p>
                         <p>We regret to inform you that your request to join UniTutor has been rejected.</p>
                         <p>Please verify that the university ID and details you provided are correct. If you believe this is an error, please contact our support team for assistance.</p>
                         <p>Best regards,<br>UniTutor Team</p>
                        </div>
                        <div class='email-footer'>
                            <p>This is an automated message, please do not reply directly to this email.</p>
                            <p>If you have any questions, feel free to contact our support team at support@unitutor.com.</p>
                        </div>
                    </div>
                </body>
                </html>";
            await _emailService.SendEmailAsync(tutor.universityMail, emailSubject, emailMessage);

            return Ok();
        }

        [HttpGet("Studentdetails/{id}")]
        public async Task<ActionResult<StudentViewMoreDto>> GetStudentDetails(int id)
        {
            var student = _admin.GetStudentById(id);

            if (student == null)
            {
                return NotFound("Student not found");
            }

            var studentViewMoreDto = new StudentViewMoreDto
            {
                _id = student._id,
                firstName = student.firstName,
                lastName = student.lastName,
                email = student.email,
                grade = student.grade,
                schoolName = student.schoolName,
                address = student.address,
                district = student.district,
                phoneNumber = student.phoneNumber,
                numberofcomplain = student.numberofcomplain,
                ProfileUrl = student.ProfileUrl,
                CreatedAt = student.CreatedAt

              
            };

            return Ok(studentViewMoreDto);
        }
        [HttpGet("Tutordetails/{id}")]
        public async Task<ActionResult<TutorViewMoreDto>> GetTutorDetails(int id)
        {
            var tutor = _admin.GetTutorById(id);

            if (tutor == null)
            {
                return NotFound("tutor not found");
            }

            var tutorViewMoreDto = new TutorViewMoreDto
            {
                _id = tutor._id,
                firstName = tutor.firstName,
                lastName = tutor.lastName,
                occupation = tutor.occupation,
                address = tutor.address,
                district = tutor.district,
                phoneNumber = tutor.phoneNumber,
                universityMail = tutor.universityMail,
                qualifications = tutor.qualifications,
                cv = tutor.cv,
                universityID = tutor.universityID,
                ProfileUrl = tutor.ProfileUrl,
                CreatedAt = tutor.CreatedAt,
                Verified = tutor.Verified

            };

            return Ok(tutorViewMoreDto);
        }
    }
    

}
