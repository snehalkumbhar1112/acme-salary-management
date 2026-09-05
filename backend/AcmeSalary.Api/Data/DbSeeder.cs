using AcmeSalary.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace AcmeSalary.Api.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (await context.Employees.AnyAsync())
        {
            return;
        }

        var countries = new List<Country>
        {
            new() { Name = "India", Code = "IN", CurrencyCode = "INR" },
            new() { Name = "United States", Code = "US", CurrencyCode = "USD" },
            new() { Name = "United Kingdom", Code = "GB", CurrencyCode = "GBP" },
            new() { Name = "Germany", Code = "DE", CurrencyCode = "EUR" },
            new() { Name = "Canada", Code = "CA", CurrencyCode = "CAD" },
            new() { Name = "Australia", Code = "AU", CurrencyCode = "AUD" }
        };

        context.Countries.AddRange(countries);

        var departments = new List<Department>
        {
            new() { Name = "Engineering" },
            new() { Name = "Human Resources" },
            new() { Name = "Finance" },
            new() { Name = "Sales" },
            new() { Name = "Marketing" },
            new() { Name = "Operations" },
            new() { Name = "IT" },
            new() { Name = "Legal" }
        };

        context.Departments.AddRange(departments);

        await context.SaveChangesAsync();

        var firstNames = new[]
        {
            "Aarav", "Aditi", "Arjun", "Ananya", "Rahul",
            "Priya", "Rohan", "Sneha", "Vikram", "Neha",
            "Michael", "Emma", "Daniel", "Olivia", "James",
            "Sophia", "William", "Emily", "David", "Charlotte"
        };

        var lastNames = new[]
        {
            "Sharma", "Patil", "Kumbhar", "Joshi", "Deshmukh",
            "Kulkarni", "Pawar", "More", "Jadhav", "Kadam",
            "Smith", "Johnson", "Brown", "Taylor", "Wilson",
            "Anderson", "Thomas", "Martin", "Clark", "Lewis"
        };

        var jobTitles = new[]
        {
            "Software Engineer",
            "Senior Software Engineer",
            "Business Analyst",
            "Financial Analyst",
            "HR Specialist",
            "Sales Executive",
            "Marketing Specialist",
            "Operations Manager",
            "IT Administrator",
            "Legal Associate"
        };

        var statuses = new[]
        {
            "Active",
            "Active",
            "Active",
            "Active",
            "On Leave"
        };

        var random = new Random(20260905);

        var employees = new List<Employee>(10000);
        var salaries = new List<Salary>(10000);

        var baseDate = new DateTime(2026, 1, 1);

        for (var i = 1; i <= 10000; i++)
        {
            var country = countries[(i - 1) % countries.Count];
            var department = departments[(i - 1) % departments.Count];

            var firstName = firstNames[(i - 1) % firstNames.Length];
            var lastName = lastNames[(i - 1) % lastNames.Length];

            var employee = new Employee
            {
                EmployeeCode = $"EMP{i:00000}",
                FirstName = firstName,
                LastName = lastName,
                Email = $"employee{i:00000}@acme.example",
                CountryId = country.Id,
                DepartmentId = department.Id,
                JobTitle = jobTitles[(i - 1) % jobTitles.Length],
                EmploymentStatus = statuses[random.Next(statuses.Length)],
                HireDate = baseDate.AddDays(-random.Next(30, 3650)),
                CreatedAt = baseDate,
                UpdatedAt = baseDate
            };

            employees.Add(employee);
        }

        context.Employees.AddRange(employees);

        await context.SaveChangesAsync();

        foreach (var employee in employees)
        {
            var country = countries.First(x => x.Id == employee.CountryId);

            decimal baseSalary;

            if (country.CurrencyCode == "INR")
            {
                baseSalary = random.Next(400000, 1800000);
            }
            else if (country.CurrencyCode == "USD")
            {
                baseSalary = random.Next(50000, 180000);
            }
            else if (country.CurrencyCode == "GBP")
            {
                baseSalary = random.Next(40000, 130000);
            }
            else if (country.CurrencyCode == "EUR")
            {
                baseSalary = random.Next(45000, 140000);
            }
            else if (country.CurrencyCode == "CAD")
            {
                baseSalary = random.Next(50000, 150000);
            }
            else
            {
                baseSalary = random.Next(55000, 160000);
            }

            var bonus = Math.Round(baseSalary * (decimal)(random.Next(5, 21) / 100.0), 2);

            salaries.Add(new Salary
            {
                EmployeeId = employee.Id,
                BaseSalary = baseSalary,
                Bonus = bonus,
                CurrencyCode = country.CurrencyCode,
                EffectiveFrom = new DateTime(2026, 1, 1),
                EffectiveTo = null,
                ChangeReason = "Initial salary",
                CreatedAt = baseDate
            });
        }

        context.Salaries.AddRange(salaries);

        await context.SaveChangesAsync();
    }
}
