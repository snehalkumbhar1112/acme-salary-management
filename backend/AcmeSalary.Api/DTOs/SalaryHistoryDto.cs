namespace AcmeSalary.Api.DTOs;

public class SalaryHistoryDto
{
    public int Id { get; set; }
    public decimal BaseSalary { get; set; }
    public decimal Bonus { get; set; }
    public string CurrencyCode { get; set; } = string.Empty;
    public DateTime EffectiveFrom { get; set; }
    public DateTime? EffectiveTo { get; set; }
    public string ChangeReason { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
