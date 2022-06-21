using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserGroups",
                columns: table => new
                {
                    UserGroupId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGroups", x => x.UserGroupId);
                });

            migrationBuilder.CreateTable(
                name: "UserStates",
                columns: table => new
                {
                    UserStateId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStates", x => x.UserStateId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Login = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BlockedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastUpdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserGroupId = table.Column<int>(type: "int", nullable: false),
                    UserStateId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Users_UserGroups_UserGroupId",
                        column: x => x.UserGroupId,
                        principalTable: "UserGroups",
                        principalColumn: "UserGroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_UserStates_UserStateId",
                        column: x => x.UserStateId,
                        principalTable: "UserStates",
                        principalColumn: "UserStateId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "UserGroups",
                columns: new[] { "UserGroupId", "Code", "Description" },
                values: new object[,]
                {
                    { 1, 0, "Regular user." },
                    { 2, 1, "Administrator. User with additional rights." }
                });

            migrationBuilder.InsertData(
                table: "UserStates",
                columns: new[] { "UserStateId", "Code", "Description" },
                values: new object[,]
                {
                    { 1, 0, "Deleted user." },
                    { 2, 1, "Active user." }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "BlockedDate", "CreatedDate", "LastUpdate", "Login", "Password", "UserGroupId", "UserStateId" },
                values: new object[] { 1, null, new DateTime(2022, 6, 21, 11, 23, 4, 644, DateTimeKind.Utc).AddTicks(986), new DateTime(2022, 6, 21, 11, 23, 4, 644, DateTimeKind.Utc).AddTicks(1470), "Admin", "AQAAAAEAACcQAAAAEOD6xq4/veVRP16vgU4e/SFOU6wQZMvK2e3RLYrcYw30HD4OoqflBzP4eaOq0ufEmw==", 2, 2 });

            migrationBuilder.CreateIndex(
                name: "IX_UserGroups_Code",
                table: "UserGroups",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Login",
                table: "Users",
                column: "Login",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserGroupId",
                table: "Users",
                column: "UserGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserStateId",
                table: "Users",
                column: "UserStateId");

            migrationBuilder.CreateIndex(
                name: "IX_UserStates_Code",
                table: "UserStates",
                column: "Code",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "UserGroups");

            migrationBuilder.DropTable(
                name: "UserStates");
        }
    }
}
