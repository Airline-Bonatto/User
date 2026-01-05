using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace User.api.Migrations
{
    /// <inheritdoc />
    public partial class Fix_UserType_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "UserTypes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "UserTypes",
                keyColumn: "userTypeId",
                keyValue: 1,
                column: "description",
                value: "Airline User");

            migrationBuilder.UpdateData(
                table: "UserTypes",
                keyColumn: "userTypeId",
                keyValue: 2,
                column: "description",
                value: "Customer User");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "description",
                table: "UserTypes");
        }
    }
}
