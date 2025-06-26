#pragma warning disable SA1200 // Using directives should be placed correctly
using Microsoft.EntityFrameworkCore.Migrations;
#pragma warning restore SA1200 // Using directives should be placed correctly

#nullable disable

namespace TodoListApp.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class TaskItemTagTableChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropForeignKey(
                name: "FK_TaskItemTags_Tags_TagId",
                table: "TaskItemTags");

            _ = migrationBuilder.DropForeignKey(
                name: "FK_TaskItemTags_TaskItems_TaskItemId",
                table: "TaskItemTags");

            _ = migrationBuilder.AddForeignKey(
                name: "FK_TaskItemTags_Tags_TagId",
                table: "TaskItemTags",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "TagId",
                onDelete: ReferentialAction.Cascade);

            _ = migrationBuilder.AddForeignKey(
                name: "FK_TaskItemTags_TaskItems_TaskItemId",
                table: "TaskItemTags",
                column: "TaskItemId",
                principalTable: "TaskItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropForeignKey(
                name: "FK_TaskItemTags_Tags_TagId",
                table: "TaskItemTags");

            _ = migrationBuilder.DropForeignKey(
                name: "FK_TaskItemTags_TaskItems_TaskItemId",
                table: "TaskItemTags");

            _ = migrationBuilder.AddForeignKey(
                name: "FK_TaskItemTags_Tags_TagId",
                table: "TaskItemTags",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "TagId");

            _ = migrationBuilder.AddForeignKey(
                name: "FK_TaskItemTags_TaskItems_TaskItemId",
                table: "TaskItemTags",
                column: "TaskItemId",
                principalTable: "TaskItems",
                principalColumn: "Id");
        }
    }
}
