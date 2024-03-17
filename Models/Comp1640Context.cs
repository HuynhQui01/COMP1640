using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Comp1640.Models;

public partial class Comp1640Context : IdentityDbContext<CustomUser>
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

    // public virtual DbSet<Role> Roles { get; set; }

    // public virtual DbSet<RoleClam> RoleClams { get; set; }

    // public virtual DbSet<User> Users { get; set; }

    // public virtual DbSet<UserClaim> UserClaims { get; set; }

    // public virtual DbSet<UserLogin> UserLogins { get; set; }

    // public virtual DbSet<UserToken> UserTokens { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:MyConnect");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Contribution>(entity =>
        {
            entity.HasOne(d => d.User).WithMany(p => p.Contributions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Contribut__UserI__571DF1D5");
        });

        modelBuilder.HasDefaultSchema("dbo");
        modelBuilder.Entity<CustomUser>(
            entity =>
            {
                entity.ToTable(name: "Users");

                // entity.HasOne(d => d.Fac).WithMany(p =>p.Users)
                // .OnDelete(DeleteBehavior.ClientSetNull)
                // .HasConstraintName("FK_User_Fac");
            }
        );
        modelBuilder.Entity<IdentityRole>(
            entity =>
            {
                entity.ToTable(name: "Role");
            }
        );
        modelBuilder.Entity<IdentityUserRole<string>>(
            entity =>
            {
                entity.ToTable(name: "UserRoles");
            }
        );
        modelBuilder.Entity<IdentityUserClaim<string>>(
            entity =>
            {
                entity.ToTable(name: "UserClaims");
            }
        );
        modelBuilder.Entity<IdentityUserToken<string>>(
            entity =>
            {
                entity.ToTable(name: "UserTokens");
            }
        );
        modelBuilder.Entity<IdentityUserLogin<string>>(
            entity =>
            {
                entity.ToTable(name: "UserLogins");
            }
        );
        modelBuilder.Entity<IdentityRoleClaim<string>>(
            entity =>
            {
                entity.ToTable(name: "RoleClams");
            }
        );

    }

    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     modelBuilder.Entity<Contribution>(entity =>
    //     {
    //         entity.HasOne(d => d.User).WithMany(p => p.Contributions)
    //             .OnDelete(DeleteBehavior.ClientSetNull)
    //             .HasConstraintName("FK__Contribut__UserI__571DF1D5");
    //     });

    //     modelBuilder.Entity<Role>(entity =>
    //     {
    //         entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
    //             .IsUnique()
    //             .HasFilter("([NormalizedName] IS NOT NULL)");
    //     });

    //     modelBuilder.Entity<User>(entity =>
    //     {
    //         entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
    //             .IsUnique()
    //             .HasFilter("([NormalizedUserName] IS NOT NULL)");

    //         entity.HasOne(d => d.Fac).WithMany(p => p.Users).HasConstraintName("FK_User_Fac");

    //         entity.HasMany(d => d.Roles).WithMany(p => p.Users)
    //             .UsingEntity<Dictionary<string, object>>(
    //                 "UserRole",
    //                 r => r.HasOne<Role>().WithMany().HasForeignKey("RoleId"),
    //                 l => l.HasOne<User>().WithMany().HasForeignKey("UserId"),
    //                 j =>
    //                 {
    //                     j.HasKey("UserId", "RoleId");
    //                     j.HasIndex(new[] { "RoleId" }, "IX_UserRoles_RoleId");
    //                 });
    //     });

    //     OnModelCreatingPartial(modelBuilder);
    // }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
