using Microsoft.EntityFrameworkCore.Migrations;

namespace PhrmaInsightCore.Migrations
{
    public partial class migration62 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PHRMA_EMPLOYEE_ISSUE_AREAS",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EMPLOYEE_NAME = table.Column<string>(nullable: true),
                    EMPLOYEE_EMAIL = table.Column<string>(nullable: true),
                    ISSUE_AREA = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PHRMA_EMPLOYEE_ISSUE_AREAS", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PHRMA_EMPLOYEE_ISSUE_AREAS");
        }
    }
}
