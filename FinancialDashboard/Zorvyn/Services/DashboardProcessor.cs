using Microsoft.EntityFrameworkCore;

public class DashboardProcessor : IDashboardProcessor
{
    private readonly AppDbContext _context;

    public DashboardProcessor(AppDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardSummaryResponse> GetDashboardSummary()
    {
        var records = _context.FinancialRecords
            .Where(x => !x.IsDeleted);

        var totalIncome = await records
            .Where(x => x.Type == "Income")
            .SumAsync(x => (decimal?)x.Amount) ?? 0;

        var totalExpense = await records
            .Where(x => x.Type == "Expense")
            .SumAsync(x => (decimal?)x.Amount) ?? 0;

        var categoryTotals = await records
            .GroupBy(x => x.Category)
            .Select(x => new CategorySummary
            {
                Category = x.Key,
                Total = x.Sum(r => r.Amount)
            })
            .ToListAsync();

        var recent = await records
            .OrderByDescending(x => x.CreatedAt)
            .Take(10)
            .ToListAsync();

        var monthlyTrends = await records
            .GroupBy(x => new { x.RecordDate.Year, x.RecordDate.Month })
            .Select(g => new MonthlyTrend
            {
                Year = g.Key.Year,
                Month = g.Key.Month,
                Income = g.Where(x => x.Type == "Income").Sum(x => x.Amount),
                Expense = g.Where(x => x.Type == "Expense").Sum(x => x.Amount)
            })
            .OrderBy(x => x.Year)
            .ThenBy(x => x.Month)
            .ToListAsync();

        var weeklyTrends = await _context.FinancialRecords
            .Where(x => !x.IsDeleted)
            .GroupBy(x => EF.Functions.DateDiffWeek(DateTime.MinValue, x.RecordDate))
            .Select(g => new WeeklyTrend
            {
                Week = g.Key,
                Income = g.Where(x => x.Type == "Income").Sum(x => (decimal?)x.Amount) ?? 0,
                Expense = g.Where(x => x.Type == "Expense").Sum(x => (decimal?)x.Amount) ?? 0
            })
            .OrderBy(x => x.Week)
            .ToListAsync();

        return new DashboardSummaryResponse
        {
            TotalIncome = totalIncome,
            TotalExpense = totalExpense,
            NetBalance = totalIncome - totalExpense,
            CategoryTotals = categoryTotals,
            RecentActivity = recent,
            MonthlyTrends = monthlyTrends,
            WeeklyTrends = weeklyTrends
        };
    }
}