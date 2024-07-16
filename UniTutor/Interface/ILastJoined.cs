

namespace UniTutor.Interface
{
    public interface ILastJoined
    {
        Task<IEnumerable<object>> GetLastJoinedUsersAsync(int count);

    }
}
