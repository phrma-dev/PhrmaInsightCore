using Microsoft.EntityFrameworkCore.Migrations;

namespace PhrmaInsightCore.Migrations
{
    public partial class migrations25 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RecurringPeriods",
                table: "KASTLE_VISITOR",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecurringPeriods",
                table: "KASTLE_VISITOR");
        }
    }
}
