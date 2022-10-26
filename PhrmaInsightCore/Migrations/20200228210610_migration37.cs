using Microsoft.EntityFrameworkCore.Migrations;

namespace PhrmaInsightCore.Migrations
{
    public partial class migration37 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SequenceId",
                table: "KASTLE_VISITOR_MODIFY",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SequenceId",
                table: "KASTLE_VISITOR_HISTORY",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SequenceId",
                table: "KASTLE_VISITOR_DELETE",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SequenceId",
                table: "KASTLE_VISITOR",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SequenceId",
                table: "KASTLE_VISITOR_MODIFY");

            migrationBuilder.DropColumn(
                name: "SequenceId",
                table: "KASTLE_VISITOR_HISTORY");

            migrationBuilder.DropColumn(
                name: "SequenceId",
                table: "KASTLE_VISITOR_DELETE");

            migrationBuilder.DropColumn(
                name: "SequenceId",
                table: "KASTLE_VISITOR");
        }
    }
}
