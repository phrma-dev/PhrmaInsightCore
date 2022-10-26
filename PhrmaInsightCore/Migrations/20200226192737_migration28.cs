using Microsoft.EntityFrameworkCore.Migrations;

namespace PhrmaInsightCore.Migrations
{
    public partial class migration28 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HostDepartment",
                table: "KASTLE_VISITOR_HISTORY",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MeetingOrganizer",
                table: "KASTLE_VISITOR_HISTORY",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HostDepartment",
                table: "KASTLE_VISITOR_HISTORY");

            migrationBuilder.DropColumn(
                name: "MeetingOrganizer",
                table: "KASTLE_VISITOR_HISTORY");
        }
    }
}
