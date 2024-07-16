namespace UniTutor.Interface
{
    public interface ICurrentUsersTotal
    {
        Task<int> GetTotalStudentsAsync();

        Task<int> GetTotalTutorsAsync();
    }
}
