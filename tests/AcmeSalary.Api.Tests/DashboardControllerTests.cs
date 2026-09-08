using AcmeSalary.Api.Controllers;
using AcmeSalary.Api.Data;
using AcmeSalary.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AcmeSalary.Api.Tests;

public class DashboardControllerTests
{
    private static AppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task GetSummary_ReturnsCorrectEmployeeCountryAndDepartmentCounts()
    {
        await using var db = CreateDbContext();

        db.Countries.AddRange(
            new Country
            {
                Id = 1,
                Name = "India",
                Code = "IN",
                CurrencyCode = "INR"
            },
            new Country
            {
                Id = 2,
                Name = "USA",
                Code = "US",
                CurrencyCode = "USD"
            });

        db.Departments.AddRange(
            new Department
            {
                Id = 1,
                Name = "Engineering"
            },
            new Department
            {
                Id = 2,
                Name = "HR"
            });

        db.Employees.AddRange(
            new Employee
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
                HireDate = new DateTime(2024, 1, 1)
            },
            new Employee
            {
                Id = 2,
                EmployeeCode = "EMP00002",
                FirstName = "Priya",
                LastName = "Patel",
                Email = "priya@example.com",
                CountryId = 2,
                DepartmentId = 2,
                JobTitle = "HR Manager",
                EmploymentStatus = "Inactive",
                HireDate = new DateTime(2023, 1, 1)
            });

        await db.SaveChangesAsync();

        var controller = new DashboardController(db);

        var result = await controller.GetSummary();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var summary = Assert.IsType<AcmeSalary.Api.DTOs.DashboardSummaryDto>(okResult.Value);

        Assert.Equal(2, summary.TotalEmployees);
        Assert.Equal(1, summary.ActiveEmployees);
        Assert.Equal(2, summary.Countries);
        Assert.Equal(2, summary.Departments);
    }

    [Fact]
public async Task GetSalaryByCountry_ReturnsCorrectSalaryTotals()
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

    var employee1 = new Employee
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

    var employee2 = new Employee
    {
        Id = 2,
        EmployeeCode = "EMP00002",
        FirstName = "Priya",
        LastName = "Patel",
        Email = "priya@example.com",
        CountryId = 1,
        DepartmentId = 1,
        JobTitle = "Senior Software Engineer",
        EmploymentStatus = "Active",
        HireDate = new DateTime(2023, 1, 1),
        Country = country,
        Department = department
    };

    db.Countries.Add(country);
    db.Departments.Add(department);
    db.Employees.AddRange(employee1, employee2);

    db.Salaries.AddRange(
        new Salary
        {
            Id = 1,
            EmployeeId = 1,
            Employee = employee1,
            BaseSalary = 800000,
            Bonus = 100000,
            CurrencyCode = "INR",
            EffectiveFrom = new DateTime(2026, 1, 1),
            EffectiveTo = null,
            ChangeReason = "Current salary",
            CreatedAt = DateTime.UtcNow
        },
        new Salary
        {
            Id = 2,
            EmployeeId = 2,
            Employee = employee2,
            BaseSalary = 1000000,
            Bonus = 150000,
            CurrencyCode = "INR",
            EffectiveFrom = new DateTime(2026, 1, 1),
            EffectiveTo = null,
            ChangeReason = "Current salary",
            CreatedAt = DateTime.UtcNow
        });

    await db.SaveChangesAsync();

    var controller = new DashboardController(db);

    var result = await controller.GetSalaryByCountry();

    var okResult = Assert.IsType<OkObjectResult>(result.Result);
    var salaryResults =
        Assert.IsType<List<AcmeSalary.Api.DTOs.SalaryByCountryDto>>(okResult.Value);

    var india = Assert.Single(salaryResults);

    Assert.Equal("India", india.Country);
    Assert.Equal("INR", india.CurrencyCode);
    Assert.Equal(2, india.EmployeeCount);
    Assert.Equal(1800000, india.TotalBaseSalary);
    Assert.Equal(250000, india.TotalBonus);
    Assert.Equal(2050000, india.TotalCompensation);
}
[Fact]
public async Task GetSalaryByDepartment_ReturnsCorrectSalaryTotals()
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

    var employee1 = new Employee
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

    var employee2 = new Employee
    {
        Id = 2,
        EmployeeCode = "EMP00002",
        FirstName = "Priya",
        LastName = "Patel",
        Email = "priya@example.com",
        CountryId = 1,
        DepartmentId = 1,
        JobTitle = "Senior Software Engineer",
        EmploymentStatus = "Active",
        HireDate = new DateTime(2023, 1, 1),
        Country = country,
        Department = department
    };

    db.Countries.Add(country);
    db.Departments.Add(department);
    db.Employees.AddRange(employee1, employee2);

    db.Salaries.AddRange(
        new Salary
        {
            Id = 1,
            EmployeeId = 1,
            Employee = employee1,
            BaseSalary = 800000,
            Bonus = 100000,
            CurrencyCode = "INR",
            EffectiveFrom = new DateTime(2026, 1, 1),
            EffectiveTo = null,
            ChangeReason = "Current salary",
            CreatedAt = DateTime.UtcNow
        },
        new Salary
        {
            Id = 2,
            EmployeeId = 2,
            Employee = employee2,
            BaseSalary = 1000000,
            Bonus = 150000,
            CurrencyCode = "INR",
            EffectiveFrom = new DateTime(2026, 1, 1),
            EffectiveTo = null,
            ChangeReason = "Current salary",
            CreatedAt = DateTime.UtcNow
        });

    await db.SaveChangesAsync();

    var controller = new DashboardController(db);

    var result = await controller.GetSalaryByDepartment();

    var okResult = Assert.IsType<OkObjectResult>(result.Result);

    var salaryResults =
        Assert.IsType<List<AcmeSalary.Api.DTOs.SalaryByDepartmentDto>>(okResult.Value);

    var engineering = Assert.Single(salaryResults);

    Assert.Equal("Engineering", engineering.Department);
    Assert.Equal("INR", engineering.CurrencyCode);
    Assert.Equal(2, engineering.EmployeeCount);
    Assert.Equal(1800000, engineering.TotalBaseSalary);
    Assert.Equal(250000, engineering.TotalBonus);
    Assert.Equal(2050000, engineering.TotalCompensation);
}
}