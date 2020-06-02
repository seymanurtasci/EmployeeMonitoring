using Microsoft.EntityFrameworkCore.Migrations;

namespace EmployeeMonitoring.Web.Migrations
{
    public partial class employee_update_again : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DaysLeft",
                table: "Employees",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DaysLeft",
                table: "Employees");
        }
    }
}
