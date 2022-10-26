using Microsoft.EntityFrameworkCore.Migrations;

namespace PhrmaInsightCore.Migrations
{
    public partial class migration24 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RecurringId",
                table: "KASTLE_VISITOR",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RecurringInterval",
                table: "KASTLE_VISITOR",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecurringId",
                table: "KASTLE_VISITOR");

            migrationBuilder.DropColumn(
                name: "RecurringInterval",
                table: "KASTLE_VISITOR");
        }
    }
}
