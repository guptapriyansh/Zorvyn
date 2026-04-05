using System.ComponentModel.DataAnnotations.Schema;

[Table("FinancialRecords")]
public class FinancialRecord
{
    public Guid Id { get; set; }

    public decimal Amount { get; set; }

    public string Type { get; set; }

    public string Category { get; set; }

    public DateTime RecordDate { get; set; }

    public string Notes { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedAt { get; set; }

    public string CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string UpdatedBy { get; set; }
}