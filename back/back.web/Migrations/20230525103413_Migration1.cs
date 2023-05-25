using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace back.web.Migrations
{
    /// <inheritdoc />
    public partial class Migration1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_user_session_SessionEntityid",
                table: "user");

            migrationBuilder.DropForeignKey(
                name: "FK_userStory_session_SessionEntityid",
                table: "userStory");

            migrationBuilder.DropIndex(
                name: "IX_userStory_SessionEntityid",
                table: "userStory");

            migrationBuilder.DropIndex(
                name: "IX_user_SessionEntityid",
                table: "user");

            migrationBuilder.DropColumn(
                name: "SessionEntityid",
                table: "userStory");

            migrationBuilder.DropColumn(
                name: "SessionEntityid",
                table: "user");

            migrationBuilder.CreateTable(
                name: "SessionEntityUserEntity",
                columns: table => new
                {
                    sessionsid = table.Column<int>(type: "integer", nullable: false),
                    usersid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionEntityUserEntity", x => new { x.sessionsid, x.usersid });
                    table.ForeignKey(
                        name: "FK_SessionEntityUserEntity_session_sessionsid",
                        column: x => x.sessionsid,
                        principalTable: "session",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SessionEntityUserEntity_user_usersid",
                        column: x => x.usersid,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SessionEntityUserStoryEntity",
                columns: table => new
                {
                    sessionsid = table.Column<int>(type: "integer", nullable: false),
                    userStoriesid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionEntityUserStoryEntity", x => new { x.sessionsid, x.userStoriesid });
                    table.ForeignKey(
                        name: "FK_SessionEntityUserStoryEntity_session_sessionsid",
                        column: x => x.sessionsid,
                        principalTable: "session",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SessionEntityUserStoryEntity_userStory_userStoriesid",
                        column: x => x.userStoriesid,
                        principalTable: "userStory",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SessionEntityUserEntity_usersid",
                table: "SessionEntityUserEntity",
                column: "usersid");

            migrationBuilder.CreateIndex(
                name: "IX_SessionEntityUserStoryEntity_userStoriesid",
                table: "SessionEntityUserStoryEntity",
                column: "userStoriesid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SessionEntityUserEntity");

            migrationBuilder.DropTable(
                name: "SessionEntityUserStoryEntity");

            migrationBuilder.AddColumn<int>(
                name: "SessionEntityid",
                table: "userStory",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SessionEntityid",
                table: "user",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_userStory_SessionEntityid",
                table: "userStory",
                column: "SessionEntityid");

            migrationBuilder.CreateIndex(
                name: "IX_user_SessionEntityid",
                table: "user",
                column: "SessionEntityid");

            migrationBuilder.AddForeignKey(
                name: "FK_user_session_SessionEntityid",
                table: "user",
                column: "SessionEntityid",
                principalTable: "session",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_userStory_session_SessionEntityid",
                table: "userStory",
                column: "SessionEntityid",
                principalTable: "session",
                principalColumn: "id");
        }
    }
}
