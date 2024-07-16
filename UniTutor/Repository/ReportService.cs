using Microsoft.EntityFrameworkCore;
using UniTutor.DataBase;
using UniTutor.DTO;
using UniTutor.Interface;
using UniTutor.Model;

namespace UniTutor.Repository
{
    public class ReportService : IReportService
    {
        private readonly ApplicationDBContext _context;

        public ReportService(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Report> CreateReportAsync(CreateReportDto createReportDto)
        {
            var report = new Report
            {
                description = createReportDto.description,
                date = DateTime.UtcNow,
                reporterId = createReportDto.reporterId,
                reportedId = createReportDto.reportedId,
                reporterType = createReportDto.reporterType,
                reportedType = createReportDto.reportedType
            };

            _context.Reports.Add(report);
            await _context.SaveChangesAsync();

            return report;
        }
        public async Task<Report> GetReportByIdAsync(int id)
        {
            return await _context.Reports.FindAsync(id);
        }


        public async Task<List<Report>> GetAllReportsAsync()
        {
            return await _context.Reports.ToListAsync();
        }

        public async Task<bool> SuspendUserAsync(int userId, string userType)
        {
            if (userType == "Student")
            {
                var student = await _context.Students.FindAsync(userId);
                if (student == null) return false;
                student.isSuspended = true;
            }
            else if (userType == "Tutor")
            {
                var tutor = await _context.Tutors.FindAsync(userId);
                if (tutor == null) return false;
                tutor.isSuspended = true;
            }
            else
            {
                return false;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RestoreUserAsync(int userId, string userType)
        {
            if (userType == "Student")
            {
                var student = await _context.Students.FindAsync(userId);
                if (student == null) return false;
                student.isSuspended = false;
            }
            else if (userType == "Tutor")
            {
                var tutor = await _context.Tutors.FindAsync(userId);
                if (tutor == null) return false;
                tutor.isSuspended = false;
            }
            else
            {
                return false;
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}