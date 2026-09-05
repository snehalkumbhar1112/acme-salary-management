namespace AcmeSalary.Api.DTOs;

public class DashboardSummaryDto
{
    public int TotalEmployees { get; set; }
    public int ActiveEmployees { get; set; }
    public int Countries { get; set; }
    public int Departments { get; set; }
}