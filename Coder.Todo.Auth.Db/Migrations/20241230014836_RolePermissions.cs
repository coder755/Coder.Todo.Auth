using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coder.Todo.Auth.Db.Migrations
{
    /// <inheritdoc />
    public partial class RolePermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GrantedPermissions");

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                columns: table => new
                {
                    RoleId = table.Column<byte[]>(type: "varbinary(16)", maxLength: 16, nullable: false),
                    PermissionId = table.Column<byte[]>(type: "varbinary(16)", maxLength: 16, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => new { x.RoleId, x.PermissionId });
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RolePermissions");

            migrationBuilder.CreateTable(
                name: "GrantedPermissions",
                columns: table => new
                {
                    RoleId = table.Column<byte[]>(type: "varbinary(16)", maxLength: 16, nullable: false),
                    PermissionId = table.Column<byte[]>(type: "varbinary(16)", maxLength: 16, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrantedPermissions", x => new { x.RoleId, x.PermissionId });
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
