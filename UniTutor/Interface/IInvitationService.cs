using UniTutor.DTO;

namespace UniTutor.Interface
{
    public interface IInvitationService
    {
        void InviteFriend(InviteRequestDto request);
        bool VerifyCode(VerifyCodeRequestDto request);
    }
}
