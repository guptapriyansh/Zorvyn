public interface IDashboardService
{
    Task<DashboardSummaryResponse> GetDashboardSummary();
}