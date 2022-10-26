using Microsoft.EntityFrameworkCore.Migrations;

namespace PhrmaInsightCore.Migrations
{
    public partial class migration29 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ASA_Region",
                table: "SCF_FORECAST",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ASA_Region",
                table: "SCF_BUDGETING",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ASA_REGION",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ASA_REGION = table.Column<string>(nullable: true),
                    LOCATIONID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ASA_REGION", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ASA_REGION");

            migrationBuilder.DropColumn(
                name: "ASA_Region",
                table: "SCF_FORECAST");

            migrationBuilder.DropColumn(
                name: "ASA_Region",
                table: "SCF_BUDGETING");
        }
    }
}
