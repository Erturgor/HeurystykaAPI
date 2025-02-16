﻿// <auto-generated />
using System;
using Heurystyka.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Heurystyka.Infrastructure.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20250111123648_start")]
    partial class start
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.0");

            modelBuilder.Entity("Heurystyka.Domain.AlgorithmParameter", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("AlgorithmResultId")
                        .HasColumnType("TEXT");

                    b.Property<string>("ParameterName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<double>("ParameterValue")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.HasIndex("AlgorithmResultId");

                    b.ToTable("AlgorithmParameters", (string)null);
                });

            modelBuilder.Entity("Heurystyka.Domain.AlgorithmResult", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("AlgorithmName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("AlgorithmResults", (string)null);
                });

            modelBuilder.Entity("Heurystyka.Domain.ReportMultiple", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("ReportMultiples");
                });

            modelBuilder.Entity("Heurystyka.Domain.ReportSingle", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("AlgorithmFunction")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("AlgorithmName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<double>("FBest")
                        .HasColumnType("REAL");

                    b.Property<string>("Parameters")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("ReportMultipleId")
                        .HasColumnType("TEXT");

                    b.Property<string>("XBest")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ReportMultipleId");

                    b.ToTable("ReportSingle", (string)null);
                });

            modelBuilder.Entity("Heurystyka.Domain.AlgorithmParameter", b =>
                {
                    b.HasOne("Heurystyka.Domain.AlgorithmResult", null)
                        .WithMany("Parameters")
                        .HasForeignKey("AlgorithmResultId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Heurystyka.Domain.ReportSingle", b =>
                {
                    b.HasOne("Heurystyka.Domain.ReportMultiple", null)
                        .WithMany("Reports")
                        .HasForeignKey("ReportMultipleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Heurystyka.Domain.AlgorithmResult", b =>
                {
                    b.Navigation("Parameters");
                });

            modelBuilder.Entity("Heurystyka.Domain.ReportMultiple", b =>
                {
                    b.Navigation("Reports");
                });
#pragma warning restore 612, 618
        }
    }
}
