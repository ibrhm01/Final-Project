using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class BlogTableUpdate5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Blogs",
                newName: "QuoteAuthorProfession");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Blogs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 5, 28, 4, 24, 9, 588, DateTimeKind.Local).AddTicks(8170),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 5, 28, 4, 21, 13, 811, DateTimeKind.Local).AddTicks(5670));

            migrationBuilder.AddColumn<string>(
                name: "DescBottom",
                table: "Blogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DescTop",
                table: "Blogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Quote",
                table: "Blogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "QuoteAuthor",
                table: "Blogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescBottom",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "DescTop",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "Quote",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "QuoteAuthor",
                table: "Blogs");

            migrationBuilder.RenameColumn(
                name: "QuoteAuthorProfession",
                table: "Blogs",
                newName: "Description");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Blogs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 5, 28, 4, 21, 13, 811, DateTimeKind.Local).AddTicks(5670),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 5, 28, 4, 24, 9, 588, DateTimeKind.Local).AddTicks(8170));
        }
    }
}
