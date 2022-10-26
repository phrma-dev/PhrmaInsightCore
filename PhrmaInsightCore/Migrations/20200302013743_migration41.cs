using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PhrmaInsightCore.Migrations
{
    public partial class migration41 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KASTLE_VISITOR_ATTENDANCE",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MeetingDate = table.Column<string>(nullable: true),
                    CheckedIn = table.Column<string>(nullable: true),
                    CheckedInDateTime = table.Column<string>(nullable: true),
                    Host = table.Column<string>(nullable: true),
                    VisitorFirstName = table.Column<string>(nullable: true),
                    VisitorLastName = table.Column<string>(nullable: true),
                    Timestamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KASTLE_VISITOR_ATTENDANCE", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KASTLE_VISITOR_ATTENDANCE");
        }
    }
}
