using UniTutor.DTO;

namespace UniTutor.Interface
{
    public interface IInvitationService
    {
        public bool InviteFriend(InviteRequestDto request);
        bool VerifyCode(VerifyCodeRequestDto request);
    }
}
