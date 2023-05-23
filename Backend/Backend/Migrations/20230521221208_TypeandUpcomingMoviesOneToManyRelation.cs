using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class TypeandUpcomingMoviesOneToManyRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ID",
                table: "UpcomingMovies",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Bios",
                newName: "Id");

            migrationBuilder.AddColumn<int>(
                name: "TypeId",
                table: "UpcomingMovies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Types",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Types", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UpcomingMovies_TypeId",
                table: "UpcomingMovies",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_UpcomingMovies_Types_TypeId",
                table: "UpcomingMovies",
                column: "TypeId",
                principalTable: "Types",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UpcomingMovies_Types_TypeId",
                table: "UpcomingMovies");

            migrationBuilder.DropTable(
                name: "Types");

            migrationBuilder.DropIndex(
                name: "IX_UpcomingMovies_TypeId",
                table: "UpcomingMovies");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "UpcomingMovies");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "UpcomingMovies",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Bios",
                newName: "ID");
        }
    }
}
