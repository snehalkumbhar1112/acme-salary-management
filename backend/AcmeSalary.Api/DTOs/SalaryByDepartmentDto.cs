namespace AcmeSalary.Api.DTOs;

public class SalaryByDepartmentDto
{
    public string Department { get; set; } = string.Empty;
    public string CurrencyCode { get; set; } = string.Empty;
    public int EmployeeCount { get; set; }
    public decimal TotalBaseSalary { get; set; }
    public decimal TotalBonus { get; set; }
    public decimal TotalCompensation { get; set; }
}