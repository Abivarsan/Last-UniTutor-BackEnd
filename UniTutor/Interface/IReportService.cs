using UniTutor.DTO;
using UniTutor.Model;

namespace UniTutor.Interface
{
    public interface IReportService
    {
        Task<Report> CreateReportAsync(CreateReportDto createReportDto);
        public Task<Report> GetReportByIdAsync(int id);

        Task<bool> SuspendUserAsync(int userId, string userType);
        Task<bool> RestoreUserAsync(int userId, string userType);

        //Getallreports metho
        Task<List<Report>> GetAllReportsAsync();

    }
}