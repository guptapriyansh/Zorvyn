using Microsoft.EntityFrameworkCore;

public class FinancialRecordProcessor : IFinancialRecordProcessor
{
    private readonly AppDbContext _context;

    public FinancialRecordProcessor(AppDbContext context)
    {
        _context = context;
    }

    public async Task CreateRecord(CreateFinancialRecordRequest request, string userEmail)
    {
        var record = new FinancialRecord
        {
            Amount = request.Amount,
            Type = request.Type,
            Category = request.Category,
            RecordDate = request.RecordDate,
            Notes = request.Notes,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = userEmail
        };

        await _context.FinancialRecords.AddAsync(record);
        await _context.SaveChangesAsync();
    }

    public async Task<List<FinancialRecord>> GetRecords(
        DateTime? startDate,
        DateTime? endDate,
        string category,
        string type)
    {
        var query = _context.FinancialRecords
            .Where(x => !x.IsDeleted)
            .AsQueryable();

        if (startDate.HasValue)
            query = query.Where(x => x.RecordDate >= startDate);

        if (endDate.HasValue)
            query = query.Where(x => x.RecordDate <= endDate);

        if (!string.IsNullOrEmpty(category))
            query = query.Where(x => x.Category == category);

        if (!string.IsNullOrEmpty(type))
            query = query.Where(x => x.Type == type);

        return await query.ToListAsync();
    }

    public async Task UpdateRecord(UpdateFinancialRecordRequest request, string userEmail)
    {
        var record = await _context.FinancialRecords
            .FirstOrDefaultAsync(x => x.Id == request.Id);

        if (record == null)
            throw new Exception("Record not found");

        record.Amount = request.Amount;
        record.Type = request.Type;
        record.Category = request.Category;
        record.RecordDate = request.RecordDate;
        record.Notes = request.Notes;

        record.UpdatedAt = DateTime.UtcNow;
        record.UpdatedBy = userEmail;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteRecord(Guid id)
    {
        var record = await _context.FinancialRecords
            .FirstOrDefaultAsync(x => x.Id == id);

        if (record == null)
            throw new Exception("Record not found");

        record.IsDeleted = true;

        await _context.SaveChangesAsync();
    }
}