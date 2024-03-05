using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Comp1640.Models;

public partial class Comp1640Context : DbContext
{
    public Comp1640Context()
    {
    }

    public Comp1640Context(DbContextOptions<Comp1640Context> options)
        : base(options)
    {
    }

    public virtual DbSet<AcademicYear> AcademicYears { get; set; }

    public virtual DbSet<Contribution> Contributions { get; set; }

    public virtual DbSet<Faculty> Faculties { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:MyConnect");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AcademicYear>(entity =>
        {
            entity.Property(e => e.Ayid).ValueGeneratedNever();
        });

        modelBuilder.Entity<Contribution>(entity =>
        {
            entity.HasOne(d => d.Feedback).WithMany(p => p.Contributions).HasConstraintName("FK_Contributions_Feedbacks");

            entity.HasOne(d => d.User).WithMany(p => p.Contributions).HasConstraintName("FK_Contributions_User");
        });

        modelBuilder.Entity<Faculty>(entity =>
        {
            entity.HasOne(d => d.Ay).WithMany(p => p.Faculties).HasConstraintName("FK_Faculties_AcademicYear");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.Property(e => e.FeedbackId).ValueGeneratedNever();
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(e => e.RoleId).ValueGeneratedNever();
            entity.Property(e => e.RoleName).IsFixedLength();
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasOne(d => d.Fac).WithMany(p => p.Users).HasConstraintName("FK_User_Faculties");

            entity.HasOne(d => d.Role).WithMany(p => p.Users).HasConstraintName("FK_User_User");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
