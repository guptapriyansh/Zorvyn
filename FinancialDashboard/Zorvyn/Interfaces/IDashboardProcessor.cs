public interface IDashboardProcessor
{
    Task<DashboardSummaryResponse> GetDashboardSummary();
}