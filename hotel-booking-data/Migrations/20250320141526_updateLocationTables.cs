using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace hotel_booking_data.Migrations
{
    /// <inheritdoc />
    public partial class updateLocationTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DistrictCode",
                table: "Hotels",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProvinceCode",
                table: "Hotels",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WardCode",
                table: "Hotels",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Hotels_DistrictCode",
                table: "Hotels",
                column: "DistrictCode");

            migrationBuilder.CreateIndex(
                name: "IX_Hotels_ProvinceCode",
                table: "Hotels",
                column: "ProvinceCode");

            migrationBuilder.CreateIndex(
                name: "IX_Hotels_WardCode",
                table: "Hotels",
                column: "WardCode");

            migrationBuilder.AddForeignKey(
                name: "FK_Hotels_Districts_DistrictCode",
                table: "Hotels",
                column: "DistrictCode",
                principalTable: "Districts",
                principalColumn: "Code");

            migrationBuilder.AddForeignKey(
                name: "FK_Hotels_Provinces_ProvinceCode",
                table: "Hotels",
                column: "ProvinceCode",
                principalTable: "Provinces",
                principalColumn: "Code");

            migrationBuilder.AddForeignKey(
                name: "FK_Hotels_Wards_WardCode",
                table: "Hotels",
                column: "WardCode",
                principalTable: "Wards",
                principalColumn: "Code");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hotels_Districts_DistrictCode",
                table: "Hotels");

            migrationBuilder.DropForeignKey(
                name: "FK_Hotels_Provinces_ProvinceCode",
                table: "Hotels");

            migrationBuilder.DropForeignKey(
                name: "FK_Hotels_Wards_WardCode",
                table: "Hotels");

            migrationBuilder.DropIndex(
                name: "IX_Hotels_DistrictCode",
                table: "Hotels");

            migrationBuilder.DropIndex(
                name: "IX_Hotels_ProvinceCode",
                table: "Hotels");

            migrationBuilder.DropIndex(
                name: "IX_Hotels_WardCode",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "DistrictCode",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "ProvinceCode",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "WardCode",
                table: "Hotels");
        }
    }
}
