using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class BannerTableUpdate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Blogs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 5, 31, 15, 9, 4, 848, DateTimeKind.Local).AddTicks(4410),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 5, 31, 2, 1, 43, 664, DateTimeKind.Local).AddTicks(70));

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Banners");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Banners");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Blogs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 5, 31, 2, 1, 43, 664, DateTimeKind.Local).AddTicks(70),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 5, 31, 15, 9, 4, 848, DateTimeKind.Local).AddTicks(4410));
        }
    }
}
