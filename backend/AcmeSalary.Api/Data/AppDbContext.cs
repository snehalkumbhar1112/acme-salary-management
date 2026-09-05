using AcmeSalary.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace AcmeSalary.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Employee> Employees => Set<Employee>();

    public DbSet<Country> Countries => Set<Country>();

    public DbSet<Department> Departments => Set<Department>();

    public DbSet<Salary> Salaries => Set<Salary>();

    public DbSet<SalaryAudit> SalaryAudits => Set<SalaryAudit>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(x => x.Code)
                .IsRequired()
                .HasMaxLength(10);

            entity.Property(x => x.CurrencyCode)
                .IsRequired()
                .HasMaxLength(10);

            entity.HasIndex(x => x.Code)
                .IsUnique();
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.HasIndex(x => x.Name)
                .IsUnique();
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.EmployeeCode)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(x => x.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(x => x.LastName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(x => x.JobTitle)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(x => x.EmploymentStatus)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasIndex(x => x.EmployeeCode)
                .IsUnique();

            entity.HasIndex(x => x.Email)
                .IsUnique();

            entity.HasIndex(x => x.CountryId);

            entity.HasIndex(x => x.DepartmentId);

            entity.HasIndex(x => x.EmploymentStatus);

            entity.HasOne(x => x.Country)
                .WithMany(x => x.Employees)
                .HasForeignKey(x => x.CountryId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Department)
                .WithMany(x => x.Employees)
                .HasForeignKey(x => x.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Salary>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.BaseSalary)
                .HasPrecision(18, 2);

            entity.Property(x => x.Bonus)
                .HasPrecision(18, 2);

            entity.Property(x => x.CurrencyCode)
                .IsRequired()
                .HasMaxLength(10);

            entity.Property(x => x.ChangeReason)
                .HasMaxLength(500);

            entity.HasIndex(x => x.EmployeeId);

            entity.HasIndex(x => x.EffectiveFrom);

            entity.HasOne(x => x.Employee)
                .WithMany(x => x.Salaries)
                .HasForeignKey(x => x.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<SalaryAudit>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.PreviousSalary)
                .HasPrecision(18, 2);

            entity.Property(x => x.NewSalary)
                .HasPrecision(18, 2);

            entity.Property(x => x.PreviousBonus)
                .HasPrecision(18, 2);

            entity.Property(x => x.NewBonus)
                .HasPrecision(18, 2);

            entity.Property(x => x.ChangeReason)
                .HasMaxLength(500);

            entity.Property(x => x.ChangedBy)
                .HasMaxLength(200);

            entity.HasIndex(x => x.EmployeeId);

            entity.HasIndex(x => x.SalaryId);

            entity.HasOne(x => x.Employee)
                .WithMany(x => x.SalaryAudits)
                .HasForeignKey(x => x.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.Salary)
                .WithMany(x => x.SalaryAudits)
                .HasForeignKey(x => x.SalaryId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
