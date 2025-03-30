using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace hotel_booking_data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateManagerTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BuninessPhone",
                table: "Managers",
                newName: "BusinessPhone");

            migrationBuilder.RenameColumn(
                name: "BuninessEmail",
                table: "Managers",
                newName: "BusinessEmail");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BusinessPhone",
                table: "Managers",
                newName: "BuninessPhone");

            migrationBuilder.RenameColumn(
                name: "BusinessEmail",
                table: "Managers",
                newName: "BuninessEmail");
        }
    }
}
