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

        public async Task<IEnumerable<ChartDataDto>> GetMonthlyChartDataAsync()
        {
            var studentData = await _context.Students.ToListAsync();
            var tutorData = await _context.Tutors.Where(t => t.Verified).ToListAsync();

            var studentGroups = studentData
                .GroupBy(s => s.CreatedAt.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    Students = g.Count()
                });

            var tutorGroups = tutorData
                .GroupBy(t => t.CreatedAt.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    Tutors = g.Count()
                });

            var combinedData = studentGroups
                .GroupJoin(tutorGroups,
                    s => s.Month,
                    t => t.Month,
                    (s, tGroup) => new { s.Month, s.Students, Tutors = tGroup.FirstOrDefault()?.Tutors ?? 0 })
                .Select(d => new ChartDataDto
                {
                    Month = new DateTime(1, d.Month, 1).ToString("MMM"),
                    Students = d.Students,
                    Tutors = d.Tutors
                }).ToList();

            return combinedData;
        }
    }
}