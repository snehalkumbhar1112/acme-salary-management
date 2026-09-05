using AcmeSalary.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AcmeSalary.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CountriesController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public CountriesController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetCountries()
    {
        var countries = await _dbContext.Countries
            .AsNoTracking()
            .OrderBy(c => c.Name)
            .Select(c => new
            {
                c.Id,
                c.Name,
                c.Code,
                c.CurrencyCode
            })
            .ToListAsync();

        return Ok(countries);
    }
}