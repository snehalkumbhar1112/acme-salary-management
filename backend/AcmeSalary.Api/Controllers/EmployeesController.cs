using AcmeSalary.Api.Data;
using AcmeSalary.Api.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AcmeSalary.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly AppDbContext _context;

    public EmployeesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResultDto<EmployeeListItemDto>>> GetEmployees(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? search = null,
        [FromQuery] int? countryId = null,
        [FromQuery] int? departmentId = null,
        [FromQuery] string? employmentStatus = null)
    {
        if (page < 1)
        {
            page = 1;
        }

        if (pageSize < 1)
        {
            pageSize = 20;
        }

        if (pageSize > 100)
        {
            pageSize = 100;
        }

        var query = _context.Employees
            .AsNoTracking()
            .Include(x => x.Country)
            .Include(x => x.Department)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.Trim();

            query = query.Where(x =>
                x.EmployeeCode.Contains(search) ||
                x.FirstName.Contains(search) ||
                x.LastName.Contains(search) ||
                x.Email.Contains(search));
        }

        if (countryId.HasValue)
        {
            query = query.Where(x => x.CountryId == countryId.Value);
        }

        if (departmentId.HasValue)
        {
            query = query.Where(x => x.DepartmentId == departmentId.Value);
        }

        if (!string.IsNullOrWhiteSpace(employmentStatus))
        {
            employmentStatus = employmentStatus.Trim();

            query = query.Where(x =>
                x.EmploymentStatus == employmentStatus);
        }

        var totalCount = await query.CountAsync();

        var totalPages = (int)Math.Ceiling(
            totalCount / (double)pageSize);

        var employees = await query
            .OrderBy(x => x.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new EmployeeListItemDto
            {
                Id = x.Id,
                EmployeeCode = x.EmployeeCode,
                FullName = x.FirstName + " " + x.LastName,
                Email = x.Email,
                Country = x.Country.Name,
                CurrencyCode = x.Country.CurrencyCode,
                Department = x.Department.Name,
                JobTitle = x.JobTitle,
                EmploymentStatus = x.EmploymentStatus,
                HireDate = x.HireDate
            })
            .ToListAsync();

        var result = new PagedResultDto<EmployeeListItemDto>
        {
            Items = employees,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = totalPages
        };

        return Ok(result);
    }
    [HttpGet("{id:int}")]
public async Task<ActionResult<EmployeeDetailDto>> GetEmployee(int id)
{
    var employee = await _context.Employees
        .AsNoTracking()
        .Include(x => x.Country)
        .Include(x => x.Department)
        .Include(x => x.Salaries)
        .FirstOrDefaultAsync(x => x.Id == id);

    if (employee == null)
    {
        return NotFound(new
        {
            message = $"Employee with id {id} was not found."
        });
    }

    var currentSalary = employee.Salaries
        .Where(x => x.EffectiveFrom <= DateTime.UtcNow)
        .OrderByDescending(x => x.EffectiveFrom)
        .FirstOrDefault();

    var result = new EmployeeDetailDto
    {
        Id = employee.Id,
        EmployeeCode = employee.EmployeeCode,
        FirstName = employee.FirstName,
        LastName = employee.LastName,
        Email = employee.Email,
        Country = employee.Country.Name,
        CountryCode = employee.Country.Code,
        CurrencyCode = employee.Country.CurrencyCode,
        Department = employee.Department.Name,
        JobTitle = employee.JobTitle,
        EmploymentStatus = employee.EmploymentStatus,
        HireDate = employee.HireDate,

        CurrentBaseSalary = currentSalary?.BaseSalary,
        CurrentBonus = currentSalary?.Bonus,
        SalaryEffectiveFrom = currentSalary?.EffectiveFrom,
        SalaryChangeReason = currentSalary?.ChangeReason
    };

    return Ok(result);
}
[HttpGet("{id:int}/salary-history")]
public async Task<ActionResult<IEnumerable<SalaryHistoryDto>>> GetSalaryHistory(int id)
{
    var employeeExists = await _context.Employees
        .AsNoTracking()
        .AnyAsync(x => x.Id == id);

    if (!employeeExists)
    {
        return NotFound(new
        {
            message = $"Employee with id {id} was not found."
        });
    }

    var history = await _context.Salaries
        .AsNoTracking()
        .Where(x => x.EmployeeId == id)
        .OrderByDescending(x => x.EffectiveFrom)
        .Select(x => new SalaryHistoryDto
        {
            Id = x.Id,
            BaseSalary = x.BaseSalary,
            Bonus = x.Bonus,
            CurrencyCode = x.CurrencyCode,
            EffectiveFrom = x.EffectiveFrom,
            EffectiveTo = x.EffectiveTo,
            ChangeReason = x.ChangeReason,
            CreatedAt = x.CreatedAt
        })
        .ToListAsync();

      return Ok(history);
}

[HttpPost("{id:int}/salary")]
public async Task<ActionResult<SalaryResponseDto>> UpdateSalary(
    int id,
    [FromBody] UpdateSalaryRequestDto request)
{
    // 1. Validate request
    if (request.BaseSalary < 0)
    {
        return BadRequest(new
        {
            message = "Base salary cannot be negative."
        });
    }

    if (request.Bonus < 0)
    {
        return BadRequest(new
        {
            message = "Bonus cannot be negative."
        });
    }

    if (string.IsNullOrWhiteSpace(request.ChangeReason))
    {
        return BadRequest(new
        {
            message = "Change reason is required."
        });
    }

    // 2. Validate effective date
    if (request.EffectiveFrom == default)
    {
        return BadRequest(new
        {
            message = "EffectiveFrom is required."
        });
    }

    // 3. Check employee exists
    var employee = await _context.Employees
        .Include(x => x.Country)
        .FirstOrDefaultAsync(x => x.Id == id);

    if (employee == null)
    {
        return NotFound(new
        {
            message = $"Employee with id {id} was not found."
        });
    }

    // 4. Start transaction
    await using var transaction = await _context.Database.BeginTransactionAsync();

    try
    {
        // 5. Find current salary
        var currentSalary = await _context.Salaries
            .Where(x =>
                x.EmployeeId == id &&
                x.EffectiveFrom <= request.EffectiveFrom &&
                (x.EffectiveTo == null ||
                 x.EffectiveTo > request.EffectiveFrom))
            .OrderByDescending(x => x.EffectiveFrom)
            .FirstOrDefaultAsync();

        // 6. Close previous salary
        if (currentSalary != null)
        {
            currentSalary.EffectiveTo = request.EffectiveFrom;
        }

        // 7. Create new salary
        var newSalary = new AcmeSalary.Api.Models.Salary
        {
            EmployeeId = id,
            BaseSalary = request.BaseSalary,
            Bonus = request.Bonus,
            CurrencyCode = employee.Country.CurrencyCode,
            EffectiveFrom = request.EffectiveFrom,
            EffectiveTo = null,
            ChangeReason = request.ChangeReason.Trim(),
            CreatedAt = DateTime.UtcNow
        };

        _context.Salaries.Add(newSalary);

        await _context.SaveChangesAsync();

        // 8. Create salary audit
        var audit = new AcmeSalary.Api.Models.SalaryAudit
        {
            SalaryId = newSalary.Id,
            EmployeeId = id,
            PreviousSalary = currentSalary?.BaseSalary ?? 0,
            NewSalary = newSalary.BaseSalary,
            PreviousBonus = currentSalary?.Bonus ?? 0,
            NewBonus = newSalary.Bonus,
            ChangeReason = newSalary.ChangeReason,
            ChangedAt = DateTime.UtcNow,
            ChangedBy = "System"
        };

        _context.SalaryAudits.Add(audit);

        await _context.SaveChangesAsync();

        // 9. Commit transaction
        await transaction.CommitAsync();

        // 10. Return response
        var response = new SalaryResponseDto
        {
            Id = newSalary.Id,
            EmployeeId = newSalary.EmployeeId,
            BaseSalary = newSalary.BaseSalary,
            Bonus = newSalary.Bonus,
            CurrencyCode = newSalary.CurrencyCode,
            EffectiveFrom = newSalary.EffectiveFrom,
            EffectiveTo = newSalary.EffectiveTo,
            ChangeReason = newSalary.ChangeReason,
            CreatedAt = newSalary.CreatedAt
        };

        return Ok(response);
    }
    catch
    {
        // Roll back everything if any operation fails
        await transaction.RollbackAsync();

        return StatusCode(500, new
        {
            message = "An error occurred while updating the salary."
        });
    }
}
[HttpGet("{id:int}/salary-audit")]
public async Task<ActionResult<IEnumerable<SalaryAuditDto>>> GetSalaryAudit(int id)
{
    var employeeExists = await _context.Employees
        .AsNoTracking()
        .AnyAsync(x => x.Id == id);

    if (!employeeExists)
    {
        return NotFound(new
        {
            message = $"Employee with id {id} was not found."
        });
    }

    var audits = await _context.SalaryAudits
        .AsNoTracking()
        .Where(x => x.EmployeeId == id)
        .OrderByDescending(x => x.ChangedAt)
        .Select(x => new SalaryAuditDto
        {
            Id = x.Id,
            SalaryId = x.SalaryId,
            EmployeeId = x.EmployeeId,
            PreviousSalary = x.PreviousSalary,
            NewSalary = x.NewSalary,
            PreviousBonus = x.PreviousBonus,
            NewBonus = x.NewBonus,
            ChangeReason = x.ChangeReason,
            ChangedAt = x.ChangedAt,
            ChangedBy = x.ChangedBy
        })
        .ToListAsync();

    return Ok(audits);
}
}
