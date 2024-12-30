using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coder.Todo.Auth.Db.Migrations
{
    /// <inheritdoc />
    public partial class UserGrantedRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GrantedRole",
                columns: table => new
                {
                    UserId = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    RoleId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrantedRole", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_GrantedRole_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GrantedRole");
        }
    }
}
