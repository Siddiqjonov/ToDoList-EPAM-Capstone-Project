#pragma warning disable SA1200 // Using directives should be placed correctly
using Microsoft.EntityFrameworkCore.Migrations;
#pragma warning restore SA1200 // Using directives should be placed correctly
#pragma warning disable SA1010 // Opening square brackets should be spaced correctly

#nullable disable

namespace TodoListApp.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    TagId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_Tags", x => x.TagId);
                });

            _ = migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Salt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false),
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_Users", x => x.Id);
                });

            _ = migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    RefreshTokenId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Expires = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRevoked = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_RefreshTokens", x => x.RefreshTokenId);
                    _ = table.ForeignKey(
                        name: "FK_RefreshTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "ToDoLists",
                columns: table => new
                {
                    TodoListId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_ToDoLists", x => x.TodoListId);
                    _ = table.ForeignKey(
                        name: "FK_ToDoLists_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "TaskItems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TodoListId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_TaskItems", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_TaskItems_ToDoLists_TodoListId",
                        column: x => x.TodoListId,
                        principalTable: "ToDoLists",
                        principalColumn: "TodoListId");
                });

            _ = migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    CommentId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    TaskItemId = table.Column<long>(type: "bigint", nullable: false),
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_Comments", x => x.CommentId);
                    _ = table.ForeignKey(
                        name: "FK_Comments_TaskItems_TaskItemId",
                        column: x => x.TaskItemId,
                        principalTable: "TaskItems",
                        principalColumn: "Id");
                    _ = table.ForeignKey(
                        name: "FK_Comments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "TaskItemTags",
                columns: table => new
                {
                    TaskItemTagId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskItemId = table.Column<long>(type: "bigint", nullable: false),
                    TagId = table.Column<long>(type: "bigint", nullable: false),
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_TaskItemTags", x => x.TaskItemTagId);
                    _ = table.ForeignKey(
                        name: "FK_TaskItemTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "TagId");
                    _ = table.ForeignKey(
                        name: "FK_TaskItemTags_TaskItems_TaskItemId",
                        column: x => x.TaskItemId,
                        principalTable: "TaskItems",
                        principalColumn: "Id");
                });

            _ = migrationBuilder.CreateIndex(
                name: "IX_Comments_TaskItemId",
                table: "Comments",
                column: "TaskItemId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_Tags_Name",
                table: "Tags",
                column: "Name",
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_TaskItems_TodoListId",
                table: "TaskItems",
                column: "TodoListId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_TaskItemTags_TagId",
                table: "TaskItemTags",
                column: "TagId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_TaskItemTags_TaskItemId_TagId",
                table: "TaskItemTags",
                columns: ["TaskItemId", "TagId"],
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_ToDoLists_UserId",
                table: "ToDoLists",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropTable(
                name: "Comments");

            _ = migrationBuilder.DropTable(
                name: "RefreshTokens");

            _ = migrationBuilder.DropTable(
                name: "TaskItemTags");

            _ = migrationBuilder.DropTable(
                name: "Tags");

            _ = migrationBuilder.DropTable(
                name: "TaskItems");

            _ = migrationBuilder.DropTable(
                name: "ToDoLists");

            _ = migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
