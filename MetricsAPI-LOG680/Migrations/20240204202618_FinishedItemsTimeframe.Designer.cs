﻿// <auto-generated />
using System;
using MetricsAPI_LOG680;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MetricsAPI_LOG680.Migrations
{
    [DbContext(typeof(ApiDbContext))]
    [Migration("20240204202618_FinishedItemsTimeframe")]
    partial class FinishedItemsTimeframe
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MetricsAPI_LOG680.DTO.ColumnActivityCount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ActiveTicketCount")
                        .HasColumnType("integer")
                        .HasColumnName("active_ticket_count");

                    b.Property<string>("ColumnName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("column_name");

                    b.HasKey("Id");

                    b.ToTable("column_activity_count");
                });

            modelBuilder.Entity("MetricsAPI_LOG680.DTO.FinishedItemsTimeframe", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("end_date");

                    b.Property<int>("FinishedItemsCount")
                        .HasColumnType("integer")
                        .HasColumnName("finished_items_count");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("start_date");

                    b.HasKey("Id");

                    b.ToTable("finished_items_timeframe");
                });

            modelBuilder.Entity("MetricsAPI_LOG680.DTO.Snapshot", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("A_faire_items")
                        .HasColumnType("integer")
                        .HasColumnName("a_faire_items");

                    b.Property<int>("Backlog_items")
                        .HasColumnType("integer")
                        .HasColumnName("backlog_items");

                    b.Property<int>("En_cours_items")
                        .HasColumnType("integer")
                        .HasColumnName("en_cours_items");

                    b.Property<int>("Revue_items")
                        .HasColumnType("integer")
                        .HasColumnName("revue_items");

                    b.Property<int>("Terminee_items")
                        .HasColumnType("integer")
                        .HasColumnName("terminee_items");

                    b.Property<DateTime>("Timestamps")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("timestamp");

                    b.Property<int>("Total_items")
                        .HasColumnType("integer")
                        .HasColumnName("total_items");

                    b.HasKey("Id");

                    b.ToTable("snapshots");
                });

            modelBuilder.Entity("MetricsAPI_LOG680.DTO.TodoItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_completed");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.HasKey("Id");

                    b.ToTable("todo_items");
                });
#pragma warning restore 612, 618
        }
    }
}
