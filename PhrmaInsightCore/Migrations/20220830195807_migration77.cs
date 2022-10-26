using Microsoft.EntityFrameworkCore.Migrations;

namespace PhrmaInsightCore.Migrations
{
    public partial class migration77 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Actuals",
                table: "SCF_BUDGETING");

            migrationBuilder.DropColumn(
                name: "BoardApproved",
                table: "SCF_BUDGETING");

            migrationBuilder.DropColumn(
                name: "Forecast",
                table: "SCF_BUDGETING");

            migrationBuilder.DropColumn(
                name: "ForecastValidThrough",
                table: "SCF_BUDGETING");

            migrationBuilder.DropColumn(
                name: "ForecastVersion",
                table: "SCF_BUDGETING");

            migrationBuilder.DropColumn(
                name: "HardCommitments",
                table: "SCF_BUDGETING");

            migrationBuilder.DropColumn(
                name: "NoteWriter",
                table: "SCF_BUDGETING");

            migrationBuilder.DropColumn(
                name: "Requisitions",
                table: "SCF_BUDGETING");

            migrationBuilder.DropColumn(
                name: "TotalCommitted",
                table: "SCF_BUDGETING");

            migrationBuilder.DropColumn(
                name: "WorkingBudget",
                table: "SCF_BUDGETING");

            migrationBuilder.AddColumn<decimal>(
                name: "NextYearBoardApproved",
                table: "SCF_BUDGETING",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "NextYearBudget",
                table: "SCF_BUDGETING",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "Show",
                table: "SCF_BUDGETING",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "ThisYearActuals",
                table: "SCF_BUDGETING",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ThisYearForecast",
                table: "SCF_BUDGETING",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NextYearBoardApproved",
                table: "SCF_BUDGETING");

            migrationBuilder.DropColumn(
                name: "NextYearBudget",
                table: "SCF_BUDGETING");

            migrationBuilder.DropColumn(
                name: "Show",
                table: "SCF_BUDGETING");

            migrationBuilder.DropColumn(
                name: "ThisYearActuals",
                table: "SCF_BUDGETING");

            migrationBuilder.DropColumn(
                name: "ThisYearForecast",
                table: "SCF_BUDGETING");

            migrationBuilder.AddColumn<decimal>(
                name: "Actuals",
                table: "SCF_BUDGETING",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "BoardApproved",
                table: "SCF_BUDGETING",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Forecast",
                table: "SCF_BUDGETING",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "ForecastValidThrough",
                table: "SCF_BUDGETING",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ForecastVersion",
                table: "SCF_BUDGETING",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "HardCommitments",
                table: "SCF_BUDGETING",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "NoteWriter",
                table: "SCF_BUDGETING",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Requisitions",
                table: "SCF_BUDGETING",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalCommitted",
                table: "SCF_BUDGETING",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "WorkingBudget",
                table: "SCF_BUDGETING",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
