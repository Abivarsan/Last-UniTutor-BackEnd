
using UniTutor.DTO;

namespace UniTutor.Interface
{
    public interface IAnalytics
    {
        Task<List<WeeklyDataDto>> GetWeeklyJoinedTutorsAsync();
        Task<List<WeeklyDataDto>> GetWeeklyJoinedStudentsAsync();
        Task<List<WeeklyDataDto>> GetWeeklyTutorRequestsAsync();
        Task<List<WeeklyDataDto>> GetWeeklyCommentsAsync();
    }
}
