using Microsoft.EntityFrameworkCore.Migrations;

namespace PhrmaInsightCore.Migrations
{
    public partial class migration51 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FEDERAL_ADVOCACY_FILES",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(nullable: true),
                    Folder = table.Column<string>(nullable: true),
                    DocumentCategory = table.Column<string>(nullable: true),
                    MeetingDate = table.Column<string>(nullable: true),
                    Members = table.Column<string>(nullable: true),
                    Year = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FEDERAL_ADVOCACY_FILES", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FEDERAL_ADVOCACY_FILES");
        }
    }
}
