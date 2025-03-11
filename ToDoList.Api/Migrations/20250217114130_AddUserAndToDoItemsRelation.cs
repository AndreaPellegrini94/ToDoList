using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoList.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddUserAndToDoItemsRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToDoItems_AspNetUsers_UserId1",
                table: "ToDoItems");

            migrationBuilder.DropIndex(
                name: "IX_ToDoItems_UserId1",
                table: "ToDoItems");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "ToDoItems");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ToDoItems",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.CreateIndex(
                name: "IX_ToDoItems_UserId",
                table: "ToDoItems",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ToDoItems_AspNetUsers_UserId",
                table: "ToDoItems",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToDoItems_AspNetUsers_UserId",
                table: "ToDoItems");

            migrationBuilder.DropIndex(
                name: "IX_ToDoItems_UserId",
                table: "ToDoItems");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "ToDoItems",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "ToDoItems",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ToDoItems_UserId1",
                table: "ToDoItems",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ToDoItems_AspNetUsers_UserId1",
                table: "ToDoItems",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
