using Microsoft.EntityFrameworkCore.Migrations;

namespace EmployeeMonitoring.Web.Migrations
{
    public partial class employee_update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AnnualLeave",
                table: "Employees",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnnualLeave",
                table: "Employees");
        }
    }
}
