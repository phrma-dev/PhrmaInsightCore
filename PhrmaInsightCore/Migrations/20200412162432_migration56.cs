using Microsoft.EntityFrameworkCore.Migrations;

namespace PhrmaInsightCore.Migrations
{
    public partial class migration56 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Date",
                table: "SHAREPOINT_USER_ACTIVITY",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Day",
                table: "SHAREPOINT_USER_ACTIVITY",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Hour",
                table: "SHAREPOINT_USER_ACTIVITY",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Month",
                table: "SHAREPOINT_USER_ACTIVITY",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Year",
                table: "SHAREPOINT_USER_ACTIVITY",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "SHAREPOINT_USER_ACTIVITY");

            migrationBuilder.DropColumn(
                name: "Day",
                table: "SHAREPOINT_USER_ACTIVITY");

            migrationBuilder.DropColumn(
                name: "Hour",
                table: "SHAREPOINT_USER_ACTIVITY");

            migrationBuilder.DropColumn(
                name: "Month",
                table: "SHAREPOINT_USER_ACTIVITY");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "SHAREPOINT_USER_ACTIVITY");
        }
    }
}
