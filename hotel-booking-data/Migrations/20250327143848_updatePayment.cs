using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace hotel_booking_data.Migrations
{
    /// <inheritdoc />
    public partial class updatePayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MethodOfPaymment",
                table: "Payments",
                newName: "MethodOfPayment");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MethodOfPayment",
                table: "Payments",
                newName: "MethodOfPaymment");
        }
    }
}
