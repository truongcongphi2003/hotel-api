using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace hotel_booking_data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabase2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RoomTypeImages_ImageId",
                table: "RoomTypeImages");

            migrationBuilder.CreateIndex(
                name: "IX_RoomTypeImages_ImageId",
                table: "RoomTypeImages",
                column: "ImageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RoomTypeImages_ImageId",
                table: "RoomTypeImages");

            migrationBuilder.CreateIndex(
                name: "IX_RoomTypeImages_ImageId",
                table: "RoomTypeImages",
                column: "ImageId",
                unique: true);
        }
    }
}
