#pragma warning disable SA1200 // Using directives should be placed correctly
using Microsoft.EntityFrameworkCore.Migrations;
#pragma warning restore SA1200 // Using directives should be placed correctly
#pragma warning disable IDE0058 // Expression value is never used

#nullable disable

namespace TodoListApp.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class TaskItemTagTableChangedAgain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskItemTags_Tags_TagId",
                table: "TaskItemTags");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskItemTags_TaskItems_TaskItemId",
                table: "TaskItemTags");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskItemTags_Tags_TagId",
                table: "TaskItemTags",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "TagId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskItemTags_TaskItems_TaskItemId",
                table: "TaskItemTags",
                column: "TaskItemId",
                principalTable: "TaskItems",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskItemTags_Tags_TagId",
                table: "TaskItemTags");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskItemTags_TaskItems_TaskItemId",
                table: "TaskItemTags");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskItemTags_Tags_TagId",
                table: "TaskItemTags",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "TagId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskItemTags_TaskItems_TaskItemId",
                table: "TaskItemTags",
                column: "TaskItemId",
                principalTable: "TaskItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
