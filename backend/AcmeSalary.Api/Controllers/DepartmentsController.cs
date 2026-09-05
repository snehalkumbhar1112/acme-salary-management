using AcmeSalary.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AcmeSalary.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DepartmentsController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public DepartmentsController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetDepartments()
    {
        var departments = await _dbContext.Departments
            .AsNoTracking()
            .OrderBy(d => d.Name)
            .Select(d => new
            {
                d.Id,
                d.Name
            })
            .ToListAsync();

        return Ok(departments);
    }
}