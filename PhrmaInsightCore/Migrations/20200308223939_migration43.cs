using Microsoft.EntityFrameworkCore.Migrations;

namespace PhrmaInsightCore.Migrations
{
    public partial class migration43 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecurringPeriods",
                table: "KASTLE_VISITOR_RECURRING");

            migrationBuilder.DropColumn(
                name: "RecurringPeriods",
                table: "KASTLE_VISITOR_MODIFY");

            migrationBuilder.DropColumn(
                name: "RecurringPeriods",
                table: "KASTLE_VISITOR_HISTORY");

            migrationBuilder.DropColumn(
                name: "RecurringPeriods",
                table: "KASTLE_VISITOR_DELETE");

            migrationBuilder.DropColumn(
                name: "RecurringPeriods",
                table: "KASTLE_VISITOR");

            migrationBuilder.AddColumn<string>(
                name: "VisitorEmail",
                table: "KASTLE_VISITOR_RECURRING",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VisitorFirstName",
                table: "KASTLE_VISITOR_RECURRING",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VisitorLastName",
                table: "KASTLE_VISITOR_RECURRING",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VisitorOrganization",
                table: "KASTLE_VISITOR_RECURRING",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VisitorEmail",
                table: "KASTLE_VISITOR_RECURRING");

            migrationBuilder.DropColumn(
                name: "VisitorFirstName",
                table: "KASTLE_VISITOR_RECURRING");

            migrationBuilder.DropColumn(
                name: "VisitorLastName",
                table: "KASTLE_VISITOR_RECURRING");

            migrationBuilder.DropColumn(
                name: "VisitorOrganization",
                table: "KASTLE_VISITOR_RECURRING");

            migrationBuilder.AddColumn<string>(
                name: "RecurringPeriods",
                table: "KASTLE_VISITOR_RECURRING",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RecurringPeriods",
                table: "KASTLE_VISITOR_MODIFY",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RecurringPeriods",
                table: "KASTLE_VISITOR_HISTORY",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RecurringPeriods",
                table: "KASTLE_VISITOR_DELETE",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RecurringPeriods",
                table: "KASTLE_VISITOR",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
