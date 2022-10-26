using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PhrmaInsightCore.Migrations
{
    public partial class migration35 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EndDate",
                table: "KASTLE_VISITOR_DELETE",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GroupId",
                table: "KASTLE_VISITOR_DELETE",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Host",
                table: "KASTLE_VISITOR_DELETE",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HostDepartment",
                table: "KASTLE_VISITOR_DELETE",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MeetingDate",
                table: "KASTLE_VISITOR_DELETE",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MeetingOrganizer",
                table: "KASTLE_VISITOR_DELETE",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NiceMeetingDate",
                table: "KASTLE_VISITOR_DELETE",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NiceMeetingTime",
                table: "KASTLE_VISITOR_DELETE",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NotifyEmail",
                table: "KASTLE_VISITOR_DELETE",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RecurringId",
                table: "KASTLE_VISITOR_DELETE",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RecurringInterval",
                table: "KASTLE_VISITOR_DELETE",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RecurringPeriods",
                table: "KASTLE_VISITOR_DELETE",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Subject",
                table: "KASTLE_VISITOR_DELETE",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Timestamp",
                table: "KASTLE_VISITOR_DELETE",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "VisitorEmail",
                table: "KASTLE_VISITOR_DELETE",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VisitorFirstName",
                table: "KASTLE_VISITOR_DELETE",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VisitorLastName",
                table: "KASTLE_VISITOR_DELETE",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VisitorOrganization",
                table: "KASTLE_VISITOR_DELETE",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "KASTLE_VISITOR_DELETE");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "KASTLE_VISITOR_DELETE");

            migrationBuilder.DropColumn(
                name: "Host",
                table: "KASTLE_VISITOR_DELETE");

            migrationBuilder.DropColumn(
                name: "HostDepartment",
                table: "KASTLE_VISITOR_DELETE");

            migrationBuilder.DropColumn(
                name: "MeetingDate",
                table: "KASTLE_VISITOR_DELETE");

            migrationBuilder.DropColumn(
                name: "MeetingOrganizer",
                table: "KASTLE_VISITOR_DELETE");

            migrationBuilder.DropColumn(
                name: "NiceMeetingDate",
                table: "KASTLE_VISITOR_DELETE");

            migrationBuilder.DropColumn(
                name: "NiceMeetingTime",
                table: "KASTLE_VISITOR_DELETE");

            migrationBuilder.DropColumn(
                name: "NotifyEmail",
                table: "KASTLE_VISITOR_DELETE");

            migrationBuilder.DropColumn(
                name: "RecurringId",
                table: "KASTLE_VISITOR_DELETE");

            migrationBuilder.DropColumn(
                name: "RecurringInterval",
                table: "KASTLE_VISITOR_DELETE");

            migrationBuilder.DropColumn(
                name: "RecurringPeriods",
                table: "KASTLE_VISITOR_DELETE");

            migrationBuilder.DropColumn(
                name: "Subject",
                table: "KASTLE_VISITOR_DELETE");

            migrationBuilder.DropColumn(
                name: "Timestamp",
                table: "KASTLE_VISITOR_DELETE");

            migrationBuilder.DropColumn(
                name: "VisitorEmail",
                table: "KASTLE_VISITOR_DELETE");

            migrationBuilder.DropColumn(
                name: "VisitorFirstName",
                table: "KASTLE_VISITOR_DELETE");

            migrationBuilder.DropColumn(
                name: "VisitorLastName",
                table: "KASTLE_VISITOR_DELETE");

            migrationBuilder.DropColumn(
                name: "VisitorOrganization",
                table: "KASTLE_VISITOR_DELETE");
        }
    }
}
