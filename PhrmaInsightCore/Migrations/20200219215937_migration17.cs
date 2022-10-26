using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PhrmaInsightCore.Migrations
{
    public partial class migration17 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KASTLE_VISITOR",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupId = table.Column<int>(nullable: false),
                    MeetingDate = table.Column<DateTime>(nullable: false),
                    Subject = table.Column<string>(nullable: true),
                    Host = table.Column<string>(nullable: true),
                    NotifyEmail = table.Column<string>(nullable: true),
                    VisitorFirstName = table.Column<string>(nullable: true),
                    VisitorLastName = table.Column<string>(nullable: true),
                    VisitorEmail = table.Column<string>(nullable: true),
                    VisitorOrganization = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KASTLE_VISITOR", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KASTLE_VISITOR");
        }
    }
}
