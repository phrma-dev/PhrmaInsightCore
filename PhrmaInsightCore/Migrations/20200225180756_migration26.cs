using Microsoft.EntityFrameworkCore.Migrations;

namespace PhrmaInsightCore.Migrations
{
    public partial class migration26 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KASTLE_VISITOR_HISTORY",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupId = table.Column<string>(nullable: true),
                    UId = table.Column<string>(nullable: true),
                    RecurringId = table.Column<string>(nullable: true),
                    RecurringInterval = table.Column<string>(nullable: true),
                    RecurringPeriods = table.Column<string>(nullable: true),
                    MeetingDate = table.Column<string>(nullable: true),
                    NiceMeetingDate = table.Column<string>(nullable: true),
                    NiceMeetingTime = table.Column<string>(nullable: true),
                    Subject = table.Column<string>(nullable: true),
                    Host = table.Column<string>(nullable: true),
                    NotifyEmail = table.Column<string>(nullable: true),
                    VisitorFirstName = table.Column<string>(nullable: true),
                    VisitorLastName = table.Column<string>(nullable: true),
                    VisitorEmail = table.Column<string>(nullable: true),
                    VisitorOrganization = table.Column<string>(nullable: true),
                    EndDate = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KASTLE_VISITOR_HISTORY", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SHAREPOINT_USER_ACTIVITY_ERROR",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Error = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SHAREPOINT_USER_ACTIVITY_ERROR", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KASTLE_VISITOR_HISTORY");

            migrationBuilder.DropTable(
                name: "SHAREPOINT_USER_ACTIVITY_ERROR");
        }
    }
}
