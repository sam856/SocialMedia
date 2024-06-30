using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Migrations
{
    /// <inheritdoc />
    public partial class UpdateData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Data",
                table: "Stories");

            migrationBuilder.AddColumn<string>(
                name: "storyId",
                table: "Media",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Media_storyId",
                table: "Media",
                column: "storyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Media_Stories_storyId",
                table: "Media",
                column: "storyId",
                principalTable: "Stories",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Media_Stories_storyId",
                table: "Media");

            migrationBuilder.DropIndex(
                name: "IX_Media_storyId",
                table: "Media");

            migrationBuilder.DropColumn(
                name: "storyId",
                table: "Media");

            migrationBuilder.AddColumn<byte[]>(
                name: "Data",
                table: "Stories",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }
    }
}
