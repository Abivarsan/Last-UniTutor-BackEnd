using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore.Metadata;
using UniTutor.DataBase;
using UniTutor.DTO;
using UniTutor.Interface;
using UniTutor.Model;

namespace UniTutor.Services
{
    public class InvitationService : IInvitationService
    {
        private readonly IEmailService _emailService;
        private readonly ApplicationDBContext _DBcontext;

        public InvitationService(ApplicationDBContext context, IEmailService emailService)
        {

            _DBcontext = context;
            _emailService = emailService;
        }
        //    public void InviteFriend(InviteRequestDto request)
        //    {
        //        var verificationCode = Guid.NewGuid().ToString();
        //        var emailSubject = "Invitation to Join UniTutor!";
        //        var emailMessage = $@"
        //    <html>
        //    <head>
        //    <style>
        //    body {{
        //        font-family: Arial, sans-serif;
        //        line-height: 1.6;
        //    }}
        //    .email-container {{
        //        width: 100%;
        //        padding: 20px;
        //        background-color: lightblue;
        //    }}
        //    .email-header {{
        //        background-color: #024A7B;
        //        color: white;
        //        padding: 10px 20px;
        //        text-align: center;
        //    }}
        //    .email-content {{
        //        background-color: lightblue;
        //        padding: 20px;
        //        margin-top: 20px;
        //    }}
        //    .email-footer {{
        //        margin-top: 20px;
        //        text-align: center;
        //        font-size: 0.9em;
        //        color: #777;
        //    }}
        //    </style>
        //    </head>
        //    <body>
        //    <div class='email-container'>
        //    <div class='email-header'>
        //    <h1>UniTutor</h1>
        //    </div>
        //    <div class='email-content'>
        //        <p>Dear,</p>
        //        <p>We are excited to invite you to join UniTutor! Our platform connects you with students who need your expertise and guidance.</p>
        //        <p>One of our tutors has invited you to join, and as a special welcome, both you and the inviting tutor will receive free coins upon your successful registration and verification of your account.</p>
        //        <p>Please use the following verification code to join: <b>{verificationCode}</b></p>
        //        <p>Once registered, paste this code in the coin wallet section to receive your free coins.</p>
        //        <p>If you have any questions or need assistance, please feel free to reach out to our support team at any time.</p>
        //        <p>Best regards,<br>UniTutor Team</p>
        //    </div>
        //    <div class='email-footer'>
        //        <p>This is an automated message, please do not reply directly to this email.</p>
        //        <p>If you have any questions, feel free to contact our support team at support@unitutor.com.</p>
        //    </div>
        //    </div>
        //    </body>
        //    </html>";

        //        _emailService.SendEmailAsync(request.Email, emailSubject, emailMessage);

        //        var invitation = new Invitation
        //        {
        //            Email = request.Email,
        //            VerificationCode = verificationCode,
        //            InvitedById = request.InvitedById,
        //        };
        //        _DBcontext.Invitations.Add(invitation);
        //        _DBcontext.SaveChanges();
        //    }


        public bool VerifyCode(VerifyCodeRequestDto request)
        {
            var invitation = _DBcontext.Invitations.FirstOrDefault(i => i.VerificationCode == request.VerificationCode);

            if (invitation == null || invitation.IsVerified)
            {
                return false;
            }

            var invitedTutor = _DBcontext.Tutors.FirstOrDefault(t => t.universityMail == invitation.Email);
            var inviter = _DBcontext.Tutors.FirstOrDefault(t => t._id == invitation.InvitedById);

            if (invitedTutor != null && inviter != null)
            {
                invitedTutor.Coins += 50; // Add coins to invited user
                inviter.Coins += 50; // Add coins to inviter

                // Add transaction record for invited tutor
                var invitedTutorTransaction = new Transaction
                {
                    tutorId = invitedTutor._id,
                    Coins = 50, // Adjust as per your business logic
                    timestamp = DateTime.UtcNow,
                    Description = "Coins awarded for being invited",
                    StripeSessionId = ""
                };
                _DBcontext.Transactions.Add(invitedTutorTransaction);

                // Add transaction record for inviter tutor
                var inviterTransaction = new Transaction
                {
                    tutorId = inviter._id,
                    Coins = 50, // Adjust as per your business logic
                    timestamp = DateTime.UtcNow,
                    Description = "Coins awarded for inviting a tutor",
                    StripeSessionId = ""
                };
                _DBcontext.Transactions.Add(inviterTransaction);

                invitation.IsVerified = true;

                _DBcontext.SaveChanges();
                return true;
            }

            return false;
        }
        public void InviteFriend(InviteRequestDto request)
        {
            // Check if the tutor already exists in the Tutors table
            var existingTutor = _DBcontext.Tutors.SingleOrDefault(t => t.universityMail == request.Email);
            if (existingTutor != null)
            {
                throw new InvalidOperationException("A tutor with this email already exists.");
            }

            // Check if an invitation already exists for this email in the Invitations table
            var existingInvitation = _DBcontext.Invitations.SingleOrDefault(i => i.Email == request.Email);
            if (existingInvitation != null)
            {
                throw new InvalidOperationException("An invitation for this email already exists.");
            }

            // Generate verification code and email content
            var verificationCode = Guid.NewGuid().ToString();
            var emailSubject = "Invitation to Join UniTutor!";
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
                    <p>Dear,</p>
                    <p>We are excited to invite you to join UniTutor! Our platform connects you with students who need your expertise and guidance.</p>
                    <p>One of our tutors has invited you to join, and as a special welcome, both you and the inviting tutor will receive free coins upon your successful registration and verification of your account.</p>
                    <p>Please use the following verification code to join: <b>{verificationCode}</b></p>
                    <p>Once registered, paste this code in the coin wallet section to receive your free coins.</p>
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

            // Send the invitation email
            _emailService.SendEmailAsync(request.Email, emailSubject, emailMessage);

            // Save the invitation to the database
            var invitation = new Invitation
            {
                Email = request.Email,
                VerificationCode = verificationCode,
                InvitedById = request.InvitedById,
            };
            _DBcontext.Invitations.Add(invitation);
            _DBcontext.SaveChanges();
        }

    }
}
