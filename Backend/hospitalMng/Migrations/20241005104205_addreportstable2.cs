using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace hospitalMng.Migrations
{
    /// <inheritdoc />
    public partial class addreportstable2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FilePath",
                table: "Reports",
                newName: "FileName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FileName",
                table: "Reports",
                newName: "FilePath");
        }
    }
}
