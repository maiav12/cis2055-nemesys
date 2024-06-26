﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Nemesys.Models.Contexts;

#nullable disable

namespace Nemesys.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240422130932_New")]
    partial class New
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Nemesys.Models.Investigation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateOfAction")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InvestigatorEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InvestigatorPhone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NearMissReportId")
                        .HasColumnType("int");

                    b.Property<string>("ReportStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("NearMissReportId");

                    b.ToTable("Investigations");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            DateOfAction = new DateTime(2024, 4, 22, 13, 9, 31, 394, DateTimeKind.Utc).AddTicks(3331),
                            Description = "Example Investigation Description",
                            InvestigatorEmail = "investigator@email.com",
                            InvestigatorPhone = "987-654-3210",
                            NearMissReportId = 1,
                            ReportStatus = "Open"
                        });
                });

            modelBuilder.Entity("Nemesys.Models.NearMissReport", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateAndTimeSpotted")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateOfReport")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OptionalPhoto")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ReporterEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ReporterPhone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TypeOfHazard")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Upvotes")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("NearMissReports");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            DateAndTimeSpotted = new DateTime(2024, 4, 22, 13, 9, 31, 394, DateTimeKind.Utc).AddTicks(2935),
                            DateOfReport = new DateTime(2024, 4, 22, 13, 9, 31, 394, DateTimeKind.Utc).AddTicks(2931),
                            Description = "Example Description",
                            Location = "Example Location",
                            OptionalPhoto = "/images/seed2.jpg",
                            ReporterEmail = "example@email.com",
                            ReporterPhone = "123-456-7890",
                            Status = "Open",
                            Title = "Example Title",
                            TypeOfHazard = "UnsafeAct",
                            Upvotes = 0
                        });
                });

            modelBuilder.Entity("Nemesys.Models.Investigation", b =>
                {
                    b.HasOne("Nemesys.Models.NearMissReport", "NearMissReport")
                        .WithMany()
                        .HasForeignKey("NearMissReportId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("NearMissReport");
                });
#pragma warning restore 612, 618
        }
    }
}
