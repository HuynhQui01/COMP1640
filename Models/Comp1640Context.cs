using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore;

namespace Comp1640.Models;

public partial class Comp1640Context : IdentityDbContext
{
    public Comp1640Context()
    {
    }

    public Comp1640Context(DbContextOptions<Comp1640Context> options)
        : base(options)
    {
    }

    public virtual DbSet<AcademicYear> AcademicYears { get; set; }

    // public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    // public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    // public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    // public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    // public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    // public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<Contribution> Contributions { get; set; }

    public virtual DbSet<Faculty> Faculties { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //     => optionsBuilder.UseSqlServer("Name=ConnectionStrings:MyConnect");

    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     modelBuilder.Entity<AcademicYear>(entity =>
    //     {
    //         entity.Property(e => e.Ayid).ValueGeneratedNever();
    //     });

    //     modelBuilder.Entity<AspNetRole>(entity =>
    //     {
    //         entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
    //             .IsUnique()
    //             .HasFilter("([NormalizedName] IS NOT NULL)");
    //     });

    //     modelBuilder.Entity<AspNetUser>(entity =>
    //     {
    //         entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
    //             .IsUnique()
    //             .HasFilter("([NormalizedUserName] IS NOT NULL)");

    //         entity.HasMany(d => d.Roles).WithMany(p => p.Users)
    //             .UsingEntity<Dictionary<string, object>>(
    //                 "AspNetUserRole",
    //                 r => r.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
    //                 l => l.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
    //                 j =>
    //                 {
    //                     j.HasKey("UserId", "RoleId");
    //                     j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
    //                 });
    //     });

    //     modelBuilder.Entity<Contribution>(entity =>
    //     {
    //         entity.HasKey(e => e.ConId).HasName("PK__Contribu__E19F47A9BD972774");

    //         entity.Property(e => e.ConId).ValueGeneratedNever();

    //         entity.HasOne(d => d.Fac).WithMany(p => p.Contributions).HasConstraintName("FK__Contribut__FacID__6FE99F9F");

    //         entity.HasOne(d => d.Feedback).WithMany(p => p.Contributions).HasConstraintName("FK__Contribut__Feedb__656C112C");

    //         entity.HasOne(d => d.User).WithMany(p => p.Contributions)
    //             .OnDelete(DeleteBehavior.ClientSetNull)
    //             .HasConstraintName("FK__Contribut__UserI__6477ECF3");
    //     });

    //     modelBuilder.Entity<Faculty>(entity =>
    //     {
    //         entity.HasKey(e => e.FacId).HasName("PK__Facultie__815081E860760D47");

    //         entity.Property(e => e.FacId).ValueGeneratedNever();

    //         entity.HasOne(d => d.Ay).WithMany(p => p.Faculties).HasConstraintName("FK__Faculties__AYId__6EF57B66");
    //     });

    //     modelBuilder.Entity<Feedback>(entity =>
    //     {
    //         entity.Property(e => e.FeedbackId).ValueGeneratedNever();

    //         entity.HasOne(d => d.User).WithMany(p => p.Feedbacks).HasConstraintName("FK__Feedbacks__UserI__6754599E");
    //     });

    //     OnModelCreatingPartial(modelBuilder);
    // }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    public DbSet<Comp1640.Models.AcademicYear> AcademicYear { get; set; } = default!;
}
