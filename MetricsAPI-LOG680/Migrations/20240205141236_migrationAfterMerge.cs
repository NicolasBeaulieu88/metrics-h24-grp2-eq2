using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MetricsAPI_LOG680.Migrations
{
    /// <inheritdoc />
    public partial class migrationAfterMerge : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PRDiscussions",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    pr_id = table.Column<string>(type: "text", nullable: false),
                    username = table.Column<string>(type: "text", nullable: false),
                    repository = table.Column<string>(type: "text", nullable: false),
                    saved_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    comments = table.Column<int>(type: "integer", nullable: false),
                    reviews = table.Column<int>(type: "integer", nullable: false),
                    reviewRequests = table.Column<int>(type: "integer", nullable: false),
                    totalDiscussions = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRDiscussions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "PRFlowRatio",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    username = table.Column<string>(type: "text", nullable: false),
                    repository = table.Column<string>(type: "text", nullable: false),
                    saved_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    openedPR = table.Column<int>(type: "integer", nullable: false),
                    closedPR = table.Column<int>(type: "integer", nullable: false),
                    flowRatio = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRFlowRatio", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "PRLeadTime",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    pr_id = table.Column<string>(type: "text", nullable: false),
                    username = table.Column<string>(type: "text", nullable: false),
                    repository = table.Column<string>(type: "text", nullable: false),
                    saved_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    closed_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    lead_time = table.Column<TimeSpan>(type: "interval", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRLeadTime", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "PRMergedTime",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    pr_id = table.Column<string>(type: "text", nullable: false),
                    username = table.Column<string>(type: "text", nullable: false),
                    repository = table.Column<string>(type: "text", nullable: false),
                    saved_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    merged_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    merged_time = table.Column<TimeSpan>(type: "interval", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRMergedTime", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "PRSize",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    pr_id = table.Column<string>(type: "text", nullable: false),
                    username = table.Column<string>(type: "text", nullable: false),
                    repository = table.Column<string>(type: "text", nullable: false),
                    saved_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    additions = table.Column<int>(type: "integer", nullable: false),
                    deletions = table.Column<int>(type: "integer", nullable: false),
                    totalChanges = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRSize", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PRDiscussions");

            migrationBuilder.DropTable(
                name: "PRFlowRatio");

            migrationBuilder.DropTable(
                name: "PRLeadTime");

            migrationBuilder.DropTable(
                name: "PRMergedTime");

            migrationBuilder.DropTable(
                name: "PRSize");
        }
    }
}
