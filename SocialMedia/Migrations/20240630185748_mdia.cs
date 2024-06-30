using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Migrations
{
    /// <inheritdoc />
    public partial class mdia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Media_Stories_storyId",
                table: "Media");

            migrationBuilder.AddForeignKey(
                name: "FK_Media_Stories_storyId",
                table: "Media",
                column: "storyId",
                principalTable: "Stories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Media_Stories_storyId",
                table: "Media");

            migrationBuilder.AddForeignKey(
                name: "FK_Media_Stories_storyId",
                table: "Media",
                column: "storyId",
                principalTable: "Stories",
                principalColumn: "Id");
        }
    }
}
