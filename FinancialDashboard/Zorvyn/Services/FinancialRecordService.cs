public class FinancialRecordService : IFinancialRecordService
{
    private readonly IFinancialRecordProcessor _processor;

    public FinancialRecordService(IFinancialRecordProcessor processor)
    {
        _processor = processor;
    }

    public Task CreateRecord(CreateFinancialRecordRequest request, string userEmail)
        => _processor.CreateRecord(request, userEmail);

    public Task<List<FinancialRecord>> GetRecords(
        DateTime? startDate,
        DateTime? endDate,
        string category,
        string type)
        => _processor.GetRecords(startDate, endDate, category, type);

    public Task UpdateRecord(UpdateFinancialRecordRequest request, string userEmail)
        => _processor.UpdateRecord(request, userEmail);

    public Task DeleteRecord(Guid id)
        => _processor.DeleteRecord(id);
}