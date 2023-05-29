using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace back.web.Migrations
{
    /// <inheritdoc />
    public partial class Migration3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "session",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    identifier = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_session", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "userStory",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    description = table.Column<string>(type: "text", nullable: false),
                    estimatedCost = table.Column<int>(type: "integer", nullable: false),
                    tasks = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userStory", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "userStoryProposition",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    description = table.Column<string>(type: "text", nullable: false),
                    tasks = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userStoryProposition", x => x.id);
                });

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

            migrationBuilder.CreateTable(
                name: "note",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    note = table.Column<int>(type: "integer", nullable: false),
                    UserStoryPropositionEntityid = table.Column<int>(type: "integer", nullable: false),
                    UserEntityid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_note", x => x.id);
                    table.ForeignKey(
                        name: "FK_note_userStoryProposition_UserStoryPropositionEntityid",
                        column: x => x.UserStoryPropositionEntityid,
                        principalTable: "userStoryProposition",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_note_user_UserEntityid",
                        column: x => x.UserEntityid,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_note_UserEntityid",
                table: "note",
                column: "UserEntityid");

            migrationBuilder.CreateIndex(
                name: "IX_note_UserStoryPropositionEntityid",
                table: "note",
                column: "UserStoryPropositionEntityid");

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
                name: "note");

            migrationBuilder.DropTable(
                name: "SessionEntityUserEntity");

            migrationBuilder.DropTable(
                name: "SessionEntityUserStoryEntity");

            migrationBuilder.DropTable(
                name: "userStoryProposition");

            migrationBuilder.DropTable(
                name: "user");

            migrationBuilder.DropTable(
                name: "session");

            migrationBuilder.DropTable(
                name: "userStory");
        }
    }
}
