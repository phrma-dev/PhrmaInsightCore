using Microsoft.EntityFrameworkCore.Migrations;

namespace PhrmaInsightCore.Migrations
{
    public partial class migration46 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Actuals",
                table: "SCF_FORECAST");

            migrationBuilder.DropColumn(
                name: "ForecastBudget",
                table: "SCF_FORECAST");

            migrationBuilder.DropColumn(
                name: "POBalance",
                table: "SCF_FORECAST");

            migrationBuilder.DropColumn(
                name: "Requisitions",
                table: "SCF_FORECAST");

            migrationBuilder.DropColumn(
                name: "TotalCommitted",
                table: "SCF_FORECAST");

            migrationBuilder.DropColumn(
                name: "WorkingBudget",
                table: "SCF_FORECAST");

            migrationBuilder.AddColumn<decimal>(
                name: "NextYearBoardApproved",
                table: "SCF_FORECAST",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "Show",
                table: "SCF_FORECAST",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "ThisYearActuals",
                table: "SCF_FORECAST",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ThisYearForecast",
                table: "SCF_FORECAST",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NextYearBoardApproved",
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

            migrationBuilder.AddColumn<decimal>(
                name: "Actuals",
                table: "SCF_FORECAST",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ForecastBudget",
                table: "SCF_FORECAST",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "POBalance",
                table: "SCF_FORECAST",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Requisitions",
                table: "SCF_FORECAST",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalCommitted",
                table: "SCF_FORECAST",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "WorkingBudget",
                table: "SCF_FORECAST",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
