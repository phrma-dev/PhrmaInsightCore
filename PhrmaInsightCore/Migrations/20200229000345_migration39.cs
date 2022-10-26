using Microsoft.EntityFrameworkCore.Migrations;

namespace PhrmaInsightCore.Migrations
{
    public partial class migration39 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImpexiumGroup",
                table: "KASTLE_VISITOR_RECURRING",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImpexiumGroupCode",
                table: "KASTLE_VISITOR_RECURRING",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImpexiumGroup",
                table: "KASTLE_VISITOR_RECURRING");

            migrationBuilder.DropColumn(
                name: "ImpexiumGroupCode",
                table: "KASTLE_VISITOR_RECURRING");
        }
    }
}
