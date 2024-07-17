using UniTutor.DTO;

namespace UniTutor.Interface
{
    public interface IChartData
    {
        Task<IEnumerable<ChartDataDto>> GetMonthlyChartDataAsync();
    }
}
