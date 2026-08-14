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

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Designation> Designations { get; set; }

    public virtual DbSet<EmployeeType> EmployeeTypes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.IdNo);

            entity.HasIndex(e => e.Name, "IX_Departments_Name").IsUnique();
        });

        modelBuilder.Entity<Designation>(entity =>
        {
            entity.HasKey(e => e.IdNo);

            entity.HasIndex(e => e.Name, "IX_Designations_Name").IsUnique();
        });

        modelBuilder.Entity<EmployeeType>(entity =>
        {
            entity.HasKey(e => e.TypeId);

            entity.HasIndex(e => e.TypeName, "IX_EmployeeTypes_TypeName").IsUnique();
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(e => e.UserName, "IX_Users_UserName").IsUnique();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
