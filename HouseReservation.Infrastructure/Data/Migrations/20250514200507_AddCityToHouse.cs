using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HouseReservationApp.Migrations
{
    /// <inheritdoc />
    public partial class AddCityToHouse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Houses",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Houses");
        }
    }
}
