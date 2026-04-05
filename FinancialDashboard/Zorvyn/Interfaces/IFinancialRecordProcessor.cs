public interface IFinancialRecordProcessor
{
    Task CreateRecord(CreateFinancialRecordRequest request, string userEmail);

    Task<List<FinancialRecord>> GetRecords(
        DateTime? startDate,
        DateTime? endDate,
        string category,
        string type);

    Task UpdateRecord(UpdateFinancialRecordRequest request, string userEmail);

    Task DeleteRecord(Guid id);
}