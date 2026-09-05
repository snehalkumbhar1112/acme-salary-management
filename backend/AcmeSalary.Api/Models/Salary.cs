namespace AcmeSalary.Api.Models;

public class Salary
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public decimal BaseSalary { get; set; }

    public decimal Bonus { get; set; }

    public string CurrencyCode { get; set; } = string.Empty;

    public DateTime EffectiveFrom { get; set; }

    public DateTime? EffectiveTo { get; set; }

    public string ChangeReason { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public Employee Employee { get; set; } = null!;

    public ICollection<SalaryAudit> SalaryAudits { get; set; } = new List<SalaryAudit>();
}
