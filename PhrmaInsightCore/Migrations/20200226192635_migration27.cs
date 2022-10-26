using Microsoft.EntityFrameworkCore.Migrations;

namespace PhrmaInsightCore.Migrations
{
    public partial class migration27 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HostDepartment",
                table: "KASTLE_VISITOR",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MeetingOrganizer",
                table: "KASTLE_VISITOR",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HostDepartment",
                table: "KASTLE_VISITOR");

            migrationBuilder.DropColumn(
                name: "MeetingOrganizer",
                table: "KASTLE_VISITOR");
        }
    }
}
