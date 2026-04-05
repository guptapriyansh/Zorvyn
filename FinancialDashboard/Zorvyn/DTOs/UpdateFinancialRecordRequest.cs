public class UpdateFinancialRecordRequest
{
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public string Type { get; set; }
    public string Category { get; set; }
    public DateTime RecordDate { get; set; }
    public string Notes { get; set; }
}