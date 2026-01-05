using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace User.api.Migrations
{
    /// <inheritdoc />
    public partial class Change_AirlineUsers_Name : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Password",
                table: "AirlineUsers",
                newName: "password");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "AirlineUsers",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "AirlineUsers",
                newName: "lastName");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "AirlineUsers",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Document",
                table: "AirlineUsers",
                newName: "document");

            migrationBuilder.RenameColumn(
                name: "AirlineUserId",
                table: "AirlineUsers",
                newName: "airlineUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "password",
                table: "AirlineUsers",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "AirlineUsers",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "lastName",
                table: "AirlineUsers",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "AirlineUsers",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "document",
                table: "AirlineUsers",
                newName: "Document");

            migrationBuilder.RenameColumn(
                name: "airlineUserId",
                table: "AirlineUsers",
                newName: "AirlineUserId");
        }
    }
}
