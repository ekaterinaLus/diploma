﻿// <auto-generated />
using System;
using Diploma.DataBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace DataStore.Migrations
{
    [DbContext(typeof(BusinessUniversityContext))]
    [Migration("20190629154042_Added history of subscribes")]
    partial class Addedhistoryofsubscribes
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("DataStore.Entities.Company", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ContactInformation")
                        .HasMaxLength(2000);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("DataStore.Entities.Event", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(300);

                    b.Property<decimal?>("Cost");

                    b.Property<DateTime>("Date");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.HasKey("Id");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("DataStore.Entities.EventsTags", b =>
                {
                    b.Property<int>("EventId");

                    b.Property<int>("TagId");

                    b.HasKey("EventId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("EventsTags");
                });

            modelBuilder.Entity("DataStore.Entities.News", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Annotation");

                    b.Property<DateTime>("Date");

                    b.Property<string>("Header")
                        .IsRequired()
                        .HasMaxLength(350);

                    b.Property<string>("Link");

                    b.Property<int?>("SectionId");

                    b.Property<string>("Text")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("SectionId");

                    b.ToTable("News");
                });

            modelBuilder.Entity("DataStore.Entities.NewsTags", b =>
                {
                    b.Property<int>("NewsId");

                    b.Property<int>("TagId");

                    b.HasKey("NewsId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("NewsTags");
                });

            modelBuilder.Entity("DataStore.Entities.NewsType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("NewsTypes");
                });

            modelBuilder.Entity("DataStore.Entities.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("CostCurrent");

                    b.Property<decimal>("CostFull");

                    b.Property<DateTime>("Date");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<string>("FileName");

                    b.Property<DateTime?>("FinishDate");

                    b.Property<int>("InitializerId");

                    b.Property<bool>("IsClosed");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(300);

                    b.Property<string>("Risks");

                    b.Property<int>("Stage");

                    b.Property<DateTime?>("StartDate");

                    b.HasKey("Id");

                    b.HasIndex("InitializerId");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("DataStore.Entities.Projects.ProjectRate", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<int>("ProjectId");

                    b.Property<int>("Rate");

                    b.HasKey("UserId", "ProjectId");

                    b.HasIndex("ProjectId");

                    b.ToTable("ProjectsRates");
                });

            modelBuilder.Entity("DataStore.Entities.Projects.SubscribesHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("UserId");

                    b.Property<int>("ViewsCount");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("SubscribesHistories");
                });

            modelBuilder.Entity("DataStore.Entities.ProjectsCompanies", b =>
                {
                    b.Property<int>("ProjectId");

                    b.Property<int>("CompanyId");

                    b.HasKey("ProjectId", "CompanyId");

                    b.HasIndex("CompanyId");

                    b.ToTable("ProjectsCompanies");
                });

            modelBuilder.Entity("DataStore.Entities.ProjectsTags", b =>
                {
                    b.Property<int>("ProjectId");

                    b.Property<int>("TagId");

                    b.HasKey("ProjectId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("ProjectsTags");
                });

            modelBuilder.Entity("DataStore.Entities.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.HasKey("Id");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("DataStore.Entities.University", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ContactInformation")
                        .HasMaxLength(2000);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("Universities");
                });

            modelBuilder.Entity("DataStore.Entities.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<int?>("CompanyId");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<int?>("UniversityId");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.HasIndex("UniversityId");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("DataStore.Entities.UsersTags", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<int>("TagId");

                    b.HasKey("UserId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("UsersTags");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("DataStore.Entities.EventsTags", b =>
                {
                    b.HasOne("DataStore.Entities.Event", "Event")
                        .WithMany("Tags")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DataStore.Entities.Tag", "Tag")
                        .WithMany("Events")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DataStore.Entities.News", b =>
                {
                    b.HasOne("DataStore.Entities.NewsType", "Section")
                        .WithMany()
                        .HasForeignKey("SectionId");
                });

            modelBuilder.Entity("DataStore.Entities.NewsTags", b =>
                {
                    b.HasOne("DataStore.Entities.News", "News")
                        .WithMany("Tags")
                        .HasForeignKey("NewsId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DataStore.Entities.Tag", "Tag")
                        .WithMany("News")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DataStore.Entities.Project", b =>
                {
                    b.HasOne("DataStore.Entities.University", "Initializer")
                        .WithMany()
                        .HasForeignKey("InitializerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DataStore.Entities.Projects.ProjectRate", b =>
                {
                    b.HasOne("DataStore.Entities.Project", "Project")
                        .WithMany()
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DataStore.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DataStore.Entities.Projects.SubscribesHistory", b =>
                {
                    b.HasOne("DataStore.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("DataStore.Entities.ProjectsCompanies", b =>
                {
                    b.HasOne("DataStore.Entities.Company", "Company")
                        .WithMany("Projects")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DataStore.Entities.Project", "Project")
                        .WithMany("Sponsors")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DataStore.Entities.ProjectsTags", b =>
                {
                    b.HasOne("DataStore.Entities.Project", "Project")
                        .WithMany("Tags")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DataStore.Entities.Tag", "Tag")
                        .WithMany("Projects")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DataStore.Entities.User", b =>
                {
                    b.HasOne("DataStore.Entities.Company")
                        .WithMany("Employees")
                        .HasForeignKey("CompanyId");

                    b.HasOne("DataStore.Entities.University")
                        .WithMany("Employees")
                        .HasForeignKey("UniversityId");
                });

            modelBuilder.Entity("DataStore.Entities.UsersTags", b =>
                {
                    b.HasOne("DataStore.Entities.Tag", "Tag")
                        .WithMany("Users")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DataStore.Entities.User", "User")
                        .WithMany("Tags")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("DataStore.Entities.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("DataStore.Entities.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DataStore.Entities.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("DataStore.Entities.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
