using Microsoft.EntityFrameworkCore.Migrations;

namespace PhrmaInsightCore.Migrations
{
    public partial class migration45 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SHAREPOINT_USER_ACTIVITY_ERROR");

            migrationBuilder.CreateTable(
                name: "SHAREPOINT_USER_ACTIVITY_SECTION",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Section = table.Column<string>(nullable: true),
                    Template = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SHAREPOINT_USER_ACTIVITY_SECTION", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SHAREPOINT_USER_ACTIVITY_SECTION");

            migrationBuilder.CreateTable(
                name: "SHAREPOINT_USER_ACTIVITY_ERROR",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Error = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SHAREPOINT_USER_ACTIVITY_ERROR", x => x.Id);
                });
        }
    }
}
