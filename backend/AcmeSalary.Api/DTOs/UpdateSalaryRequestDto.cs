namespace AcmeSalary.Api.DTOs;

public class UpdateSalaryRequestDto
{
    public decimal BaseSalary { get; set; }

    public decimal Bonus { get; set; }

    public string ChangeReason { get; set; } = string.Empty;

    public DateTime EffectiveFrom { get; set; }
}