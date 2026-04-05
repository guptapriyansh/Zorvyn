public class DashboardService : IDashboardService
{
    private readonly IDashboardProcessor _processor;

    public DashboardService(IDashboardProcessor processor)
    {
        _processor = processor;
    }

    public Task<DashboardSummaryResponse> GetDashboardSummary()
        => _processor.GetDashboardSummary();
}