﻿// <auto-generated />
using System;
using Comp1640.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Comp1640.Migrations
{
    [DbContext(typeof(Comp1640Context))]
    [Migration("20240326214950_updateadadad")]
    partial class updateadadad
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("dbo")
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Comp1640.Models.AcademicYear", b =>
                {
                    b.Property<int>("Ayid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("AYId");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Ayid"));

                    b.Property<DateTime?>("CloseDate")
                        .HasColumnType("date");

                    b.Property<DateTime?>("FinalCloseDate")
                        .HasColumnType("date");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Ayid");

                    b.ToTable("AcademicYear", "dbo");
                });

            modelBuilder.Entity("Comp1640.Models.Contribution", b =>
                {
                    b.Property<int>("ConId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ConID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ConId"));

                    b.Property<string>("Buplic")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ConName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("FacId")
                        .HasColumnType("int");

                    b.Property<string>("Filepath")
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ImageFilePath")
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime>("SubmitDate")
                        .HasColumnType("datetime");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("UserID");

                    b.HasKey("ConId");

                    b.HasIndex(new[] { "FacId" }, "IX_Contributions_FacId");

                    b.HasIndex(new[] { "UserId" }, "IX_Contributions_UserID");

                    b.ToTable("Contributions", "dbo");
                });

            modelBuilder.Entity("Comp1640.Models.CustomUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FacName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("ProfileImagePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("Users", "dbo");
                });

            modelBuilder.Entity("Comp1640.Models.Faculty", b =>
                {
                    b.Property<int>("FacId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("FacID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FacId"));

                    b.Property<int?>("Ayid")
                        .HasColumnType("int")
                        .HasColumnName("AYId");

                    b.Property<string>("FacName")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.HasKey("FacId");

                    b.HasIndex(new[] { "Ayid" }, "IX_Faculties_AYId");

                    b.ToTable("Faculties", "dbo");
                });

            modelBuilder.Entity("Comp1640.Models.Feedback", b =>
                {
                    b.Property<int>("FeedbackId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FeedbackId"));

                    b.Property<string>("Comment")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<int?>("ConId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("Fbtime")
                        .HasColumnType("datetime")
                        .HasColumnName("FBTime");

                    b.Property<string>("UserId")
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("FeedbackId");

                    b.HasIndex("UserId");

                    b.HasIndex(new[] { "ConId" }, "IX_Feedbacks_ConId");

                    b.ToTable("Feedbacks", "dbo");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("Role", "dbo");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("RoleClams", "dbo");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserClaims", "dbo");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("UserLogins", "dbo");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRoles", "dbo");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("UserTokens", "dbo");
                });

            modelBuilder.Entity("Comp1640.Models.Contribution", b =>
                {
                    b.HasOne("Comp1640.Models.Faculty", "Fac")
                        .WithMany("Contributions")
                        .HasForeignKey("FacId");

                    b.HasOne("Comp1640.Models.CustomUser", "User")
                        .WithMany("Contributions")
                        .HasForeignKey("UserId")
                        .IsRequired()
                        .HasConstraintName("FK__Contribut__UserI__571DF1D5");

                    b.Navigation("Fac");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Comp1640.Models.Faculty", b =>
                {
                    b.HasOne("Comp1640.Models.AcademicYear", "Ay")
                        .WithMany("Faculties")
                        .HasForeignKey("Ayid");

                    b.Navigation("Ay");
                });

            modelBuilder.Entity("Comp1640.Models.Feedback", b =>
                {
                    b.HasOne("Comp1640.Models.Contribution", "Con")
                        .WithMany("Feedbacks")
                        .HasForeignKey("ConId");

                    b.HasOne("Comp1640.Models.CustomUser", "User")
                        .WithMany("Feedbacks")
                        .HasForeignKey("UserId");

                    b.Navigation("Con");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Comp1640.Models.CustomUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Comp1640.Models.CustomUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Comp1640.Models.CustomUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Comp1640.Models.CustomUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Comp1640.Models.AcademicYear", b =>
                {
                    b.Navigation("Faculties");
                });

            modelBuilder.Entity("Comp1640.Models.Contribution", b =>
                {
                    b.Navigation("Feedbacks");
                });

            modelBuilder.Entity("Comp1640.Models.CustomUser", b =>
                {
                    b.Navigation("Contributions");

                    b.Navigation("Feedbacks");
                });

            modelBuilder.Entity("Comp1640.Models.Faculty", b =>
                {
                    b.Navigation("Contributions");
                });
#pragma warning restore 612, 618
        }
    }
}
