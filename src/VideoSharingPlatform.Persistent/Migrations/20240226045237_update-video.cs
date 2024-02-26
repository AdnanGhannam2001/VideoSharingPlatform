using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VideoSharingPlatform.Persistent.Migrations
{
    /// <inheritdoc />
    public partial class updatevideo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Uri",
                table: "Videos",
                newName: "VideoExt");

            migrationBuilder.RenameColumn(
                name: "Thumbnail",
                table: "Videos",
                newName: "ThumbnailExt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VideoExt",
                table: "Videos",
                newName: "Uri");

            migrationBuilder.RenameColumn(
                name: "ThumbnailExt",
                table: "Videos",
                newName: "Thumbnail");
        }
    }
}
