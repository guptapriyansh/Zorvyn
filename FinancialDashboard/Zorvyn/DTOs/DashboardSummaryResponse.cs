public class DashboardSummaryResponse
{
    public decimal TotalIncome { get; set; }
    public decimal TotalExpense { get; set; }
    public decimal NetBalance { get; set; }
    public List<CategorySummary> CategoryTotals { get; set; }
    public List<FinancialRecord> RecentActivity { get; set; }
    public List<MonthlyTrend> MonthlyTrends { get; set; }
    public List<WeeklyTrend> WeeklyTrends { get; set; }
}