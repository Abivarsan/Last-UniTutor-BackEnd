﻿using UniTutor.DTO;

namespace UniTutor.Interface
{
    public interface IInvitationService
    {
        //public bool InviteFriend(InviteRequestDto request);
       
       // public void InviteFriend(InviteRequestDto request);
        bool VerifyCode(VerifyCodeRequestDto request);
        public void InviteFriend(InviteRequestDto request);
    }
}
