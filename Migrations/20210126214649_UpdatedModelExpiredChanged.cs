using Microsoft.EntityFrameworkCore.Migrations;

namespace ExpensesTracker5.Migrations
{
    public partial class UpdatedModelExpiredChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Expired",
                table: "Expenses",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Expired",
                table: "Expenses");
        }
    }
}
