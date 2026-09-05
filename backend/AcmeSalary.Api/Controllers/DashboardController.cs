using AcmeSalary.Api.Data;
using AcmeSalary.Api.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AcmeSalary.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public DashboardController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("summary")]
    public async Task<ActionResult<DashboardSummaryDto>> GetSummary()
    {
        var result = new DashboardSummaryDto
        {
            TotalEmployees = await _dbContext.Employees.CountAsync(),

            ActiveEmployees = await _dbContext.Employees
                .CountAsync(e => e.EmploymentStatus == "Active"),

            Countries = await _dbContext.Countries.CountAsync(),

            Departments = await _dbContext.Departments.CountAsync()
        };

        return Ok(result);
    }
  
  [HttpGet("salary-by-country")]
public async Task<ActionResult<List<SalaryByCountryDto>>> GetSalaryByCountry()
{
    var salaries = await _dbContext.Salaries
        .AsNoTracking()
        .Where(s => s.EffectiveTo == null)
        .Select(s => new
        {
            Country = s.Employee.Country.Name,
            CurrencyCode = s.CurrencyCode,
            BaseSalary = s.BaseSalary,
            Bonus = s.Bonus
        })
        .ToListAsync();

    var result = salaries
        .GroupBy(s => new
        {
            s.Country,
            s.CurrencyCode
        })
        .Select(g => new SalaryByCountryDto
        {
            Country = g.Key.Country,
            CurrencyCode = g.Key.CurrencyCode,
            EmployeeCount = g.Count(),
            TotalBaseSalary = g.Sum(s => s.BaseSalary),
            TotalBonus = g.Sum(s => s.Bonus),
            TotalCompensation = g.Sum(s => s.BaseSalary + s.Bonus)
        })
        .OrderBy(x => x.Country)
        .ToList();

    return Ok(result);
}
[HttpGet("salary-by-department")]
public async Task<ActionResult<List<SalaryByDepartmentDto>>> GetSalaryByDepartment()
{
    var salaries = await _dbContext.Salaries
        .AsNoTracking()
        .Where(s => s.EffectiveTo == null)
        .Select(s => new
        {
            Department = s.Employee.Department.Name,
            CurrencyCode = s.CurrencyCode,
            BaseSalary = s.BaseSalary,
            Bonus = s.Bonus
        })
        .ToListAsync();

    var result = salaries
        .GroupBy(s => new
        {
            s.Department,
            s.CurrencyCode
        })
        .Select(g => new SalaryByDepartmentDto
{
    Department = g.Key.Department,
    CurrencyCode = g.Key.CurrencyCode,
    EmployeeCount = g.Count(),
            TotalBaseSalary = g.Sum(s => s.BaseSalary),
            TotalBonus = g.Sum(s => s.Bonus),
            TotalCompensation = g.Sum(s => s.BaseSalary + s.Bonus)
        })
        .OrderBy(x => x.Department)
        .ToList();

    return Ok(result);
}
}