using Microsoft.EntityFrameworkCore.Migrations;

namespace ExpensesTracker5.Migrations
{
    public partial class ExpiredChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Expired",
                table: "Expenses");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Expired",
                table: "Expenses",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
