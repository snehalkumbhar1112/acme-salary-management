namespace AcmeSalary.Api.Models;

public class Employee
{
    public int Id { get; set; }

    public string EmployeeCode { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public int CountryId { get; set; }

    public int DepartmentId { get; set; }

    public string JobTitle { get; set; } = string.Empty;

    public string EmploymentStatus { get; set; } = string.Empty;

    public DateTime HireDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Country Country { get; set; } = null!;

    public Department Department { get; set; } = null!;

    public ICollection<Salary> Salaries { get; set; } = new List<Salary>();

    public ICollection<SalaryAudit> SalaryAudits { get; set; } = new List<SalaryAudit>();
}
