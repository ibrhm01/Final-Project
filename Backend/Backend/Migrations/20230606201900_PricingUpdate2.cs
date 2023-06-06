using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class PricingUpdate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PricingId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PricingId",
                table: "AspNetUsers",
                column: "PricingId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Pricings_PricingId",
                table: "AspNetUsers",
                column: "PricingId",
                principalTable: "Pricings",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Pricings_PricingId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_PricingId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PricingId",
                table: "AspNetUsers");
        }
    }
}
