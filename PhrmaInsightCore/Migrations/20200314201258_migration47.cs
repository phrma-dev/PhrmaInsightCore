using Microsoft.EntityFrameworkCore.Migrations;

namespace PhrmaInsightCore.Migrations
{
    public partial class migration47 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ASA_Region",
                table: "SCF_FORECAST");

            migrationBuilder.DropColumn(
                name: "EntryCategory",
                table: "SCF_FORECAST");

            migrationBuilder.DropColumn(
                name: "NextYearBoardApproved",
                table: "SCF_FORECAST");

            migrationBuilder.DropColumn(
                name: "NextYearBudget",
                table: "SCF_FORECAST");

            migrationBuilder.DropColumn(
                name: "Show",
                table: "SCF_FORECAST");

            migrationBuilder.DropColumn(
                name: "ThisYearActuals",
                table: "SCF_FORECAST");

            migrationBuilder.DropColumn(
                name: "ThisYearForecast",
                table: "SCF_FORECAST");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "SCF_FORECAST");

            migrationBuilder.AddColumn<decimal>(
                name: "Actuals",
                table: "SCF_FORECAST",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Forecast",
                table: "SCF_FORECAST",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "ForecastValidThrough",
                table: "SCF_FORECAST",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ForecastVersion",
                table: "SCF_FORECAST",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "HardCommitments",
                table: "SCF_FORECAST",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "NoteWriter",
                table: "SCF_FORECAST",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Requisitions",
                table: "SCF_FORECAST",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "WorkingBudget",
                table: "SCF_FORECAST",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Actuals",
                table: "SCF_FORECAST");

            migrationBuilder.DropColumn(
                name: "Forecast",
                table: "SCF_FORECAST");

            migrationBuilder.DropColumn(
                name: "ForecastValidThrough",
                table: "SCF_FORECAST");

            migrationBuilder.DropColumn(
                name: "ForecastVersion",
                table: "SCF_FORECAST");

            migrationBuilder.DropColumn(
                name: "HardCommitments",
                table: "SCF_FORECAST");

            migrationBuilder.DropColumn(
                name: "NoteWriter",
                table: "SCF_FORECAST");

            migrationBuilder.DropColumn(
                name: "Requisitions",
                table: "SCF_FORECAST");

            migrationBuilder.DropColumn(
                name: "WorkingBudget",
                table: "SCF_FORECAST");

            migrationBuilder.AddColumn<string>(
                name: "ASA_Region",
                table: "SCF_FORECAST",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EntryCategory",
                table: "SCF_FORECAST",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "NextYearBoardApproved",
                table: "SCF_FORECAST",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "NextYearBudget",
                table: "SCF_FORECAST",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "Show",
                table: "SCF_FORECAST",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "ThisYearActuals",
                table: "SCF_FORECAST",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ThisYearForecast",
                table: "SCF_FORECAST",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Year",
                table: "SCF_FORECAST",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
