using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HouseReservationApp.Migrations
{
    /// <inheritdoc />
    public partial class AddImageUrlToHouse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Houses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Houses");
        }
    }
}
