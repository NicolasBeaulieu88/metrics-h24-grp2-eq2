using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MetricsAPI_LOG680.Migrations
{
    /// <inheritdoc />
    public partial class SnapshotRepo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "owner",
                table: "snapshots",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "project_id",
                table: "snapshots",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "repository_name",
                table: "snapshots",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "owner",
                table: "snapshots");

            migrationBuilder.DropColumn(
                name: "project_id",
                table: "snapshots");

            migrationBuilder.DropColumn(
                name: "repository_name",
                table: "snapshots");
        }
    }
}
