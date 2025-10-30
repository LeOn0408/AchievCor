using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AchievCor.Server.Migrations
{
    /// <inheritdoc />
    public partial class CreateUserRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ac_roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ac_roles", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ac_user_role",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    AppointedFrom = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    AppointedUntil = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    AppointmentDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    AppointedByUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ac_user_role", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ac_user_role_ac_roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "ac_roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ac_user_role_ac_users_AppointedByUserId",
                        column: x => x.AppointedByUserId,
                        principalTable: "ac_users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ac_user_role_ac_users_UserId",
                        column: x => x.UserId,
                        principalTable: "ac_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ac_user_role_AppointedByUserId",
                table: "ac_user_role",
                column: "AppointedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ac_user_role_RoleId",
                table: "ac_user_role",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_ac_user_role_UserId",
                table: "ac_user_role",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ac_user_role");

            migrationBuilder.DropTable(
                name: "ac_roles");
        }
    }
}
