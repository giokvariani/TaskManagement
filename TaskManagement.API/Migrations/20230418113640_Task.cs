using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManagement.API.Migrations
{
    /// <inheritdoc />
    public partial class Task : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Issues_Users_AssigneeId",
                table: "Issues");

            migrationBuilder.AddForeignKey(
                name: "FK_Issues_Users_AssigneeId",
                table: "Issues",
                column: "AssigneeId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Issues_Users_AssigneeId",
                table: "Issues");

            migrationBuilder.AddForeignKey(
                name: "FK_Issues_Users_AssigneeId",
                table: "Issues",
                column: "AssigneeId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
