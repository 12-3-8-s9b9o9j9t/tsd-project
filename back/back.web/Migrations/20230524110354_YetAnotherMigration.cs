using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace back.web.Migrations
{
    /// <inheritdoc />
    public partial class YetAnotherMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateTable(
                name: "session",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_session", x => x.id);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_user_session_SessionEntityid",
                table: "user");

            migrationBuilder.DropForeignKey(
                name: "FK_userStory_session_SessionEntityid",
                table: "userStory");

            migrationBuilder.DropTable(
                name: "session");

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
        }
    }
}
