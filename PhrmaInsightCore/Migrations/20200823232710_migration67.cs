using Microsoft.EntityFrameworkCore.Migrations;

namespace PhrmaInsightCore.Migrations
{
    public partial class migration67 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrgChartUsers");

            migrationBuilder.CreateTable(
                name: "EMPLOYEES",
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
                    table.PrimaryKey("PK_EMPLOYEES", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EMPLOYEES");

            migrationBuilder.CreateTable(
                name: "OrgChartUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Department = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ManagerEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MobilePhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OfficeLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OfficePhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Photo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrgChartUsers", x => x.Id);
                });
        }
    }
}
