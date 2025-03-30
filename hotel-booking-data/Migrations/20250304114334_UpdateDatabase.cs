using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace hotel_booking_data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BedType",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "CancelBeforeMinutes",
                table: "Rooms");

            migrationBuilder.RenameColumn(
                name: "RoomName",
                table: "Rooms",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "RoomCount",
                table: "Rooms",
                newName: "MaxChildren");

            migrationBuilder.RenameColumn(
                name: "PayAtHotel",
                table: "Rooms",
                newName: "IsPayAtHotel");

            migrationBuilder.RenameColumn(
                name: "MaxGuest",
                table: "Rooms",
                newName: "MaxAdults");

            migrationBuilder.AddColumn<string>(
                name: "BedTypeId",
                table: "RoomTypes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "MaxAdults",
                table: "RoomTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaxChildren",
                table: "RoomTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RoomCount",
                table: "RoomTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "BedTypes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BedName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripton = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HotelId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BedTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BedTypes_Hotels_HotelId",
                        column: x => x.HotelId,
                        principalTable: "Hotels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CancellationPolicies",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoomId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MinDaysBefore = table.Column<int>(type: "int", nullable: true),
                    FeePercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    FixedFee = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CutoffTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    IsRefundable = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CancellationPolicies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CancellationPolicies_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoomTypeBedType",
                columns: table => new
                {
                    RoomTypeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BedTypeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomTypeBedType", x => new { x.RoomTypeId, x.BedTypeId });
                    table.ForeignKey(
                        name: "FK_RoomTypeBedType_BedTypes_BedTypeId",
                        column: x => x.BedTypeId,
                        principalTable: "BedTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoomTypeBedType_RoomTypes_RoomTypeId",
                        column: x => x.RoomTypeId,
                        principalTable: "RoomTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BedTypes_HotelId",
                table: "BedTypes",
                column: "HotelId");

            migrationBuilder.CreateIndex(
                name: "IX_CancellationPolicies_RoomId",
                table: "CancellationPolicies",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomTypeBedType_BedTypeId",
                table: "RoomTypeBedType",
                column: "BedTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CancellationPolicies");

            migrationBuilder.DropTable(
                name: "RoomTypeBedType");

            migrationBuilder.DropTable(
                name: "BedTypes");

            migrationBuilder.DropColumn(
                name: "BedTypeId",
                table: "RoomTypes");

            migrationBuilder.DropColumn(
                name: "MaxAdults",
                table: "RoomTypes");

            migrationBuilder.DropColumn(
                name: "MaxChildren",
                table: "RoomTypes");

            migrationBuilder.DropColumn(
                name: "RoomCount",
                table: "RoomTypes");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Rooms",
                newName: "RoomName");

            migrationBuilder.RenameColumn(
                name: "MaxChildren",
                table: "Rooms",
                newName: "RoomCount");

            migrationBuilder.RenameColumn(
                name: "MaxAdults",
                table: "Rooms",
                newName: "MaxGuest");

            migrationBuilder.RenameColumn(
                name: "IsPayAtHotel",
                table: "Rooms",
                newName: "PayAtHotel");

            migrationBuilder.AddColumn<string>(
                name: "BedType",
                table: "Rooms",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CancelBeforeMinutes",
                table: "Rooms",
                type: "int",
                nullable: true);
        }
    }
}
