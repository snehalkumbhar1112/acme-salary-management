namespace AcmeSalary.Api.Models;

public class SalaryAudit
{
    public int Id { get; set; }

    public int SalaryId { get; set; }

    public int EmployeeId { get; set; }

    public decimal PreviousSalary { get; set; }

    public decimal NewSalary { get; set; }

    public decimal PreviousBonus { get; set; }

    public decimal NewBonus { get; set; }

    public string ChangeReason { get; set; } = string.Empty;

    public DateTime ChangedAt { get; set; }

    public string ChangedBy { get; set; } = string.Empty;

    public Salary Salary { get; set; } = null!;

    public Employee Employee { get; set; } = null!;
}
