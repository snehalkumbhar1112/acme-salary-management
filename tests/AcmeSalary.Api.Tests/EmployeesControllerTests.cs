using AcmeSalary.Api.Controllers;
using AcmeSalary.Api.Data;
using AcmeSalary.Api.DTOs;
using AcmeSalary.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AcmeSalary.Api.Tests;

public class EmployeesControllerTests
{
    private static AppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task GetEmployee_ReturnsEmployee_WhenEmployeeExists()
    {
        await using var db = CreateDbContext();

        var country = new Country
        {
            Id = 1,
            Name = "India",
            Code = "IN",
            CurrencyCode = "INR"
        };

        var department = new Department
        {
            Id = 1,
            Name = "Engineering"
        };

        var employee = new Employee
        {
            Id = 1,
            EmployeeCode = "EMP00001",
            FirstName = "Aarav",
            LastName = "Sharma",
            Email = "aarav@example.com",
            CountryId = 1,
            DepartmentId = 1,
            JobTitle = "Software Engineer",
            EmploymentStatus = "Active",
            HireDate = new DateTime(2024, 1, 1),
            Country = country,
            Department = department
        };

        db.Countries.Add(country);
        db.Departments.Add(department);
        db.Employees.Add(employee);

        db.Salaries.Add(new Salary
        {
            Id = 1,
            EmployeeId = 1,
            BaseSalary = 800000,
            Bonus = 100000,
            CurrencyCode = "INR",
            EffectiveFrom = new DateTime(2026, 1, 1),
            EffectiveTo = null,
            ChangeReason = "Initial salary",
            CreatedAt = DateTime.UtcNow
        });

        await db.SaveChangesAsync();

        var controller = new EmployeesController(db);

        var result = await controller.GetEmployee(1);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var dto = Assert.IsType<EmployeeDetailDto>(okResult.Value);

        Assert.Equal("EMP00001", dto.EmployeeCode);
        Assert.Equal("Aarav", dto.FirstName);
        Assert.Equal("Sharma", dto.LastName);
        Assert.Equal("India", dto.Country);
        Assert.Equal("Engineering", dto.Department);
        Assert.Equal("INR", dto.CurrencyCode);
        Assert.Equal(800000, dto.CurrentBaseSalary);
        Assert.Equal(100000, dto.CurrentBonus);
    }

    [Fact]
    public async Task GetEmployee_ReturnsNotFound_WhenEmployeeDoesNotExist()
    {
        await using var db = CreateDbContext();

        var controller = new EmployeesController(db);

        var result = await controller.GetEmployee(999);

        Assert.IsType<NotFoundObjectResult>(result.Result);
    }
}