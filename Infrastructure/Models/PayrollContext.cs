using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Models;

public partial class PayrollContext : DbContext
{
    public PayrollContext(DbContextOptions<PayrollContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Bank> Banks { get; set; }

    public virtual DbSet<Branch> Branches { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Designation> Designations { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EmployeeBankAccount> EmployeeBankAccounts { get; set; }

    public virtual DbSet<EmployeeSalaryComponent> EmployeeSalaryComponents { get; set; }

    public virtual DbSet<EmployeeSalaryStructure> EmployeeSalaryStructures { get; set; }

    public virtual DbSet<EmployeeType> EmployeeTypes { get; set; }

    public virtual DbSet<FinYear> FinYears { get; set; }

    public virtual DbSet<Module> Modules { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<SalaryComponent> SalaryComponents { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserModulesPolicyMapping> UserModulesPolicyMappings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Bank>(entity =>
        {
            entity.HasIndex(e => e.BankName, "IX_Banks_BankName").IsUnique();
        });

        modelBuilder.Entity<Branch>(entity =>
        {
            entity.HasIndex(e => e.Ifsccode, "IX_Branches_IFSCCode").IsUnique();

            entity.Property(e => e.Ifsccode).HasColumnName("IFSCCode");

            entity.HasOne(d => d.Bank).WithMany(p => p.Branches)
                .HasForeignKey(d => d.BankId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasIndex(e => e.CompanyCode, "IX_Companies_CompanyCode").IsUnique();

            entity.HasIndex(e => e.CompanyName, "IX_Companies_CompanyName").IsUnique();

            entity.Property(e => e.Country).HasDefaultValue("India");
            entity.Property(e => e.CurrencyCode)
                .HasDefaultValue("INR")
                .HasColumnName("currency_code");
            entity.Property(e => e.Gstin).HasColumnName("GSTIN");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.IdNo);

            entity.HasIndex(e => e.Name, "IX_Departments_Name").IsUnique();

            entity.Property(e => e.IdNo).ValueGeneratedNever();
        });

        modelBuilder.Entity<Designation>(entity =>
        {
            entity.HasKey(e => e.IdNo);

            entity.HasIndex(e => e.Name, "IX_Designations_Name").IsUnique();
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasIndex(e => new { e.CompanyId, e.EmployeeCode }, "IX_Employees_CompanyId_EmployeeCode").IsUnique();

            entity.Property(e => e.AddressLine1).HasColumnName("address_line1");
            entity.Property(e => e.AddressLine2).HasColumnName("address_line2");
            entity.Property(e => e.City).HasColumnName("city");
            entity.Property(e => e.Country)
                .HasDefaultValue("India")
                .HasColumnName("country");
            entity.Property(e => e.DateOfBirty).HasColumnType("date");
            entity.Property(e => e.Gender).HasColumnName("gender");
            entity.Property(e => e.HireDate).HasColumnType("date");
            entity.Property(e => e.IsActive).HasDefaultValue("Yes");
            entity.Property(e => e.PostalCode).HasColumnName("postal_code");
            entity.Property(e => e.State).HasColumnName("state");
            entity.Property(e => e.TerminationDate).HasColumnType("date");

            entity.HasOne(d => d.Company).WithMany(p => p.Employees)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Designation).WithMany(p => p.Employees).HasForeignKey(d => d.DesignationId);

            entity.HasOne(d => d.Manager).WithMany(p => p.InverseManager).HasForeignKey(d => d.ManagerId);
        });

        modelBuilder.Entity<EmployeeBankAccount>(entity =>
        {
            entity.HasKey(e => e.AccountId);

            entity.HasOne(d => d.Bank).WithMany(p => p.EmployeeBankAccounts)
                .HasForeignKey(d => d.BankId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Branch).WithMany(p => p.EmployeeBankAccounts)
                .HasForeignKey(d => d.BranchId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Company).WithMany(p => p.EmployeeBankAccounts).HasForeignKey(d => d.CompanyId);

            entity.HasOne(d => d.Employee).WithMany(p => p.EmployeeBankAccounts).HasForeignKey(d => d.EmployeeId);
        });

        modelBuilder.Entity<EmployeeSalaryComponent>(entity =>
        {
            entity.HasKey(e => e.SalaryComponentId);

            entity.Property(e => e.Amount)
                .HasDefaultValueSql("0")
                .HasColumnType("NUMERIC");

            entity.HasOne(d => d.Company).WithMany(p => p.EmployeeSalaryComponents)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Component).WithMany(p => p.EmployeeSalaryComponents)
                .HasForeignKey(d => d.ComponentId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Employee).WithMany(p => p.EmployeeSalaryComponents)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<EmployeeSalaryStructure>(entity =>
        {
            entity.HasKey(e => e.StructureId);

            entity.Property(e => e.AnnualCtc)
                .HasDefaultValueSql("0")
                .HasColumnType("NUMERIC")
                .HasColumnName("AnnualCTC");
            entity.Property(e => e.Basic)
                .HasDefaultValueSql("0")
                .HasColumnType("NUMERIC");
            entity.Property(e => e.CompanyId).HasColumnName("companyId");
            entity.Property(e => e.EffectiveFrom).HasColumnType("date");
            entity.Property(e => e.EffectiveTo).HasColumnType("date");
            entity.Property(e => e.IsActive).HasDefaultValue("Yes");
            entity.Property(e => e.PayFrequency).HasDefaultValue("MONTHLY");

            entity.HasOne(d => d.Company).WithMany(p => p.EmployeeSalaryStructures)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Employee).WithMany(p => p.EmployeeSalaryStructures)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<EmployeeType>(entity =>
        {
            entity.HasKey(e => e.TypeId);

            entity.HasIndex(e => e.TypeName, "IX_EmployeeTypes_TypeName").IsUnique();
        });

        modelBuilder.Entity<FinYear>(entity =>
        {
            entity.HasKey(e => e.YaarId);

            entity.ToTable("FinYear");

            entity.Property(e => e.FromDate).HasColumnType("date");
            entity.Property(e => e.ToDate).HasColumnType("date");
        });

        modelBuilder.Entity<Module>(entity =>
        {
            entity.HasKey(e => e.ModuleName);
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.IdNo);

            entity.ToTable("RefreshToken");

            entity.Property(e => e.ExpiresAt).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnType("INT");
        });

        modelBuilder.Entity<SalaryComponent>(entity =>
        {
            entity.HasKey(e => e.ComponentId);

            entity.HasIndex(e => new { e.CompanyId, e.ComponentCode }, "IX_SalaryComponents_CompanyId_ComponentCode").IsUnique();

            entity.Property(e => e.CalculationType).HasDefaultValue("FIXED");
            entity.Property(e => e.IsActive).HasDefaultValue("Yes");
            entity.Property(e => e.Taxable).HasDefaultValueSql("0");

            entity.HasOne(d => d.Company).WithMany(p => p.SalaryComponents)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(e => e.UserName, "IX_Users_UserName").IsUnique();

            entity.Property(e => e.IsAdmin).HasDefaultValue("No");
        });

        modelBuilder.Entity<UserModulesPolicyMapping>(entity =>
        {
            entity.HasKey(e => e.IdNo);

            entity.ToTable("UserModulesPolicyMapping");

            entity.HasOne(d => d.ModuleNameNavigation).WithMany(p => p.UserModulesPolicyMappings)
                .HasForeignKey(d => d.ModuleName)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.User).WithMany(p => p.UserModulesPolicyMappings)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
