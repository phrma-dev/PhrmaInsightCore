using Microsoft.EntityFrameworkCore.Migrations;

namespace PhrmaInsightCore.Migrations
{
    public partial class migration76 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MeetingOrganizerEmail",
                table: "KASTLE_VISITOR_HISTORY",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MeetingOrganizerEmail",
                table: "KASTLE_VISITOR_DELETE",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MeetingOrganizerEmail",
                table: "KASTLE_VISITOR_HISTORY");

            migrationBuilder.DropColumn(
                name: "MeetingOrganizerEmail",
                table: "KASTLE_VISITOR_DELETE");
        }
    }
}
