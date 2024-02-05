using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MetricsAPI_LOG680.Migrations
{
    /// <inheritdoc />
    public partial class SnapshotTitle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "title",
                table: "snapshots",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "title",
                table: "snapshots");
        }
    }
}
