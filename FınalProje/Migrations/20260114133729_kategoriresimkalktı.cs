using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FınalProje.Migrations
{
    /// <inheritdoc />
    public partial class kategoriresimkalktı : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Resim",
                table: "Kategoriler");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Resim",
                table: "Kategoriler",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
