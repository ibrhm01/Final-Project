using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class PricingTableUpdate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "Pricings",
                type: "float(18)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Blogs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 5, 30, 23, 38, 20, 41, DateTimeKind.Local).AddTicks(7540),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 5, 29, 21, 47, 40, 884, DateTimeKind.Local).AddTicks(4690));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Pricings",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float(18)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Blogs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 5, 29, 21, 47, 40, 884, DateTimeKind.Local).AddTicks(4690),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 5, 30, 23, 38, 20, 41, DateTimeKind.Local).AddTicks(7540));
        }
    }
}
