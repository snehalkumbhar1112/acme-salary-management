namespace AcmeSalary.Api.DTOs;

public class EmployeeDetailDto
{
    public int Id { get; set; }
    public string EmployeeCode { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string CountryCode { get; set; } = string.Empty;
    public string CurrencyCode { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string JobTitle { get; set; } = string.Empty;
    public string EmploymentStatus { get; set; } = string.Empty;
    public DateTime HireDate { get; set; }

    public decimal? CurrentBaseSalary { get; set; }
    public decimal? CurrentBonus { get; set; }
    public DateTime? SalaryEffectiveFrom { get; set; }
    public string? SalaryChangeReason { get; set; }
}
