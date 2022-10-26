using Microsoft.EntityFrameworkCore.Migrations;

namespace PhrmaInsightCore.Migrations
{
    public partial class migration66 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EMPLOYEES",
                table: "EMPLOYEES");

            migrationBuilder.RenameTable(
                name: "EMPLOYEES",
                newName: "ORG_CHART_USERS");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ORG_CHART_USERS",
                table: "ORG_CHART_USERS",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "OrgChartUsers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    MobilePhone = table.Column<string>(nullable: true),
                    OfficePhone = table.Column<string>(nullable: true),
                    OfficeLocation = table.Column<string>(nullable: true),
                    Department = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Photo = table.Column<string>(nullable: true),
                    ManagerEmail = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrgChartUsers", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrgChartUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ORG_CHART_USERS",
                table: "ORG_CHART_USERS");

            migrationBuilder.RenameTable(
                name: "ORG_CHART_USERS",
                newName: "EMPLOYEES");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EMPLOYEES",
                table: "EMPLOYEES",
                column: "Id");
        }
    }
}
