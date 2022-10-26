using Microsoft.EntityFrameworkCore.Migrations;

namespace PhrmaInsightCore.Migrations
{
    public partial class migration64 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EMPLOYEES",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
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
        }
    }
}
