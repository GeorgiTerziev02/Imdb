using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Imdb.Data.Migrations
{
    public partial class AddMoreNullValues : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeSpan>(
                name: "Duration",
                table: "Movies",
                nullable: true,
                oldClrType: typeof(TimeSpan),
                oldType: "time");

            migrationBuilder.AlterColumn<decimal>(
                name: "Budget",
                table: "Movies",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeSpan>(
                name: "Duration",
                table: "Movies",
                type: "time",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Budget",
                table: "Movies",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);
        }
    }
}
