using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Models;

public partial class PayrollContext : DbContext
{
    public PayrollContext()
    {
    }

    public PayrollContext(DbContextOptions<PayrollContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Bank> Banks { get; set; }

    public virtual DbSet<Branch> Branches { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Designation> Designations { get; set; }

    public virtual DbSet<EmployeeType> EmployeeTypes { get; set; }

    public virtual DbSet<User> Users { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlite("Data Source=D:\\Projects\\WebApi\\PayrollApi\\Payroll\\Data\\payroll.db");

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
