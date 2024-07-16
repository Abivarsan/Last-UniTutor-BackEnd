using Microsoft.EntityFrameworkCore;
using UniTutor.DataBase;
using UniTutor.DTO;
using UniTutor.Interface;

namespace UniTutor.Repository
{
    public class ChartDataRepository : IChartData
    {
        private readonly ApplicationDBContext _context;

        public ChartDataRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ChartDataDto>> GetWeeklyChartDataAsync()
        {
            var studentData = await _context.Students
                .ToListAsync();

            var tutorData = await _context.Tutors
                .Where(t => t.Verified)
                .ToListAsync();

            var studentGroups = studentData
                .GroupBy(s => s.CreatedAt.DayOfWeek)
                .Select(g => new
                {
                    DayOfWeek = g.Key.ToString(),
                    Students = g.Count()
                });

            var tutorGroups = tutorData
                .GroupBy(t => t.CreatedAt.DayOfWeek)
                .Select(g => new
                {
                    DayOfWeek = g.Key.ToString(),
                    Tutors = g.Count()
                });

            var combinedData = studentGroups
                .GroupJoin(tutorGroups,
                    s => s.DayOfWeek,
                    t => t.DayOfWeek,
                    (s, tGroup) => new { s.DayOfWeek, s.Students, Tutors = tGroup.FirstOrDefault()?.Tutors ?? 0 })
                .Select(d => new ChartDataDto
                {
                    DayOfWeek = d.DayOfWeek,
                    Students = d.Students,
                    Tutors = d.Tutors
                }).ToList();

            return combinedData;
        }
    }
}