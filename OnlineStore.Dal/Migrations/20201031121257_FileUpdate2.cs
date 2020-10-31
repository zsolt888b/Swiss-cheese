using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineStore.Dal.Migrations
{
    public partial class FileUpdate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "thumbnail",
                table: "Files",
                newName: "Thumbnail");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Thumbnail",
                table: "Files",
                newName: "thumbnail");
        }
    }
}
