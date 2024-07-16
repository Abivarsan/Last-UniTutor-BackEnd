using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using UniTutor.DataBase;
using UniTutor.DTO;
using UniTutor.Interface;

namespace UniTutor.Repository
{
    public class AnalyticsRepository : IAnalytics
    {
        private readonly ApplicationDBContext _context;

        public AnalyticsRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<List<WeeklyDataDto>> GetWeeklyJoinedTutorsAsync()
        {
            return await GetWeeklyDataAsync(_context.Tutors.Where(t => t.Verified==true), t => t.CreatedAt);
        }

        public async Task<List<WeeklyDataDto>> GetWeeklyJoinedStudentsAsync()
        {
            return await GetWeeklyDataAsync(_context.Students, s => s.CreatedAt);
        }

        public async Task<List<WeeklyDataDto>> GetWeeklyTutorRequestsAsync()
        {
            var startOfWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
            var endOfWeek = startOfWeek.AddDays(7);

            var unverifiedTutors = await _context.Tutors
                .Where(t => t.CreatedAt >= startOfWeek && t.CreatedAt < endOfWeek && !t.Verified)
                .ToListAsync();

            var groupedData = unverifiedTutors
                .GroupBy(t => t.CreatedAt.DayOfWeek)
                .Select(g => new WeeklyDataDto
                {
                    Day = g.Key.ToString(),
                    Count = g.Count()
                })
                .ToList();

            return groupedData;
        }

        public async Task<List<WeeklyDataDto>> GetWeeklyCommentsAsync()
        {
            return await GetWeeklyDataAsync(_context.Comments, c => c.timestamp);
        }

        private async Task<List<WeeklyDataDto>> GetWeeklyDataAsync<T>(IQueryable<T> query, Expression<Func<T, DateTime>> dateSelector)
            where T : class
        {
            var startDate = DateTime.Now.AddDays(-6).Date;

            var data = await query
                .Where(BuildDateFilterExpression(dateSelector, startDate))
                .ToListAsync();

            var groupedData = data
                .GroupBy(d => dateSelector.Compile()(d).DayOfWeek)
                .Select(g => new WeeklyDataDto
                {
                    Day = g.Key.ToString(),
                    Count = g.Count()
                })
                .ToList();

            return groupedData;
        }

        private static Expression<Func<T, bool>> BuildDateFilterExpression<T>(Expression<Func<T, DateTime>> dateSelector, DateTime startDate)
        {
            var parameter = dateSelector.Parameters[0];
            var body = Expression.GreaterThanOrEqual(dateSelector.Body, Expression.Constant(startDate));
            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }
    }
}
