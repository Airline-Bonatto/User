using Microsoft.EntityFrameworkCore.Migrations;

using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace User.api.Migrations
{
    /// <inheritdoc />
    public partial class Add_UserType_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "userTypeId",
                table: "AirlineUsers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "UserTypes",
                columns: table => new
                {
                    userTypeId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTypes", x => x.userTypeId);
                });

            migrationBuilder.InsertData(
                table: "UserTypes",
                column: "userTypeId",
                values: new object[]
                {
                    1,
                    2
                });

            migrationBuilder.CreateIndex(
                name: "IX_AirlineUsers_userTypeId",
                table: "AirlineUsers",
                column: "userTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AirlineUsers_UserTypes_userTypeId",
                table: "AirlineUsers",
                column: "userTypeId",
                principalTable: "UserTypes",
                principalColumn: "userTypeId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AirlineUsers_UserTypes_userTypeId",
                table: "AirlineUsers");

            migrationBuilder.DropTable(
                name: "UserTypes");

            migrationBuilder.DropIndex(
                name: "IX_AirlineUsers_userTypeId",
                table: "AirlineUsers");

            migrationBuilder.DropColumn(
                name: "userTypeId",
                table: "AirlineUsers");
        }
    }
}
