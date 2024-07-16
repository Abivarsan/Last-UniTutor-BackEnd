using Microsoft.EntityFrameworkCore.Metadata;
using UniTutor.DataBase;
using UniTutor.DTO;
using UniTutor.Interface;
using UniTutor.Model;

namespace UniTutor.Services
{
    public class InvitationService : IInvitationService
    {                                                                                                                 
        private readonly IMailService _mailService;
        private readonly ApplicationDBContext _DBcontext;

        public InvitationService(IMailService mailService, ApplicationDBContext context)
        {
            _mailService = mailService;
            _DBcontext = context;
        }

        public void InviteFriend(InviteRequestDto request)
        {
            var verificationCode = Guid.NewGuid().ToString();
            var body = $"Use this verification code to join: {verificationCode}";

            _mailService.SendEmail(request.Email, "Invitation to Join", body);

            var invitation = new Invitation
            {
                Email = request.Email,
                VerificationCode = verificationCode,
                InvitedById = request.InvitedById,
            };
            _DBcontext.Invitations.Add(invitation);
            _DBcontext.SaveChanges();
        }

        public bool VerifyCode(VerifyCodeRequestDto request)
        {
            var invitation = _DBcontext.Invitations.FirstOrDefault(i => i.VerificationCode == request.VerificationCode);

            if (invitation == null || invitation.IsVerified)
            {
                return false;
            }

            var invitedTutor = _DBcontext.Tutors.FirstOrDefault(t=> t.universityMail == invitation.Email);
            var inviter = _DBcontext.Tutors.FirstOrDefault(t => t._id == invitation.InvitedById);

            if (invitedTutor != null && inviter != null)
            {
                invitedTutor.Coins += 10; // Add coins to invited user
                inviter.Coins += 10; // Add coins to inviter

                invitation.IsVerified = true;
               
                _DBcontext.SaveChanges();
                return true;
            }

            return false;
        }
    }
}
