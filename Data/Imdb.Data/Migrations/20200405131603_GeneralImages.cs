using Microsoft.EntityFrameworkCore.Migrations;

namespace Imdb.Data.Migrations
{
    public partial class GeneralImages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GeneralImageUrl",
                table: "TvShows",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GeneralImageUrl",
                table: "Movies",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Directors",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Actors",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GeneralImageUrl",
                table: "TvShows");

            migrationBuilder.DropColumn(
                name: "GeneralImageUrl",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Directors");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Actors");
        }
    }
}
