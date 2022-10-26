using Microsoft.EntityFrameworkCore.Migrations;

namespace PhrmaInsightCore.Migrations
{
    public partial class migration3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ForecastedBudget",
                table: "PostSoftCommitmentForecast");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ForecastedBudget",
                table: "PostSoftCommitmentForecast",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
