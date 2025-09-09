using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HotelApp.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 1,
                column: "Price",
                value: 1500.0);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 2,
                columns: new[] { "Number", "Price" },
                values: new object[] { "202", 2500.0 });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 3,
                columns: new[] { "Number", "Price", "Status", "Type" },
                values: new object[] { "303", 5000.0, "Booked", "Presidental" });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "RoomId", "Number", "Price", "Status", "Type" },
                values: new object[,]
                {
                    { 4, "104", 1800.0, "Booked", "Single" },
                    { 5, "205", 2800.0, "Free", "Double" },
                    { 6, "306", 5500.0, "Free", "Presidental" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 6);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 1,
                column: "Price",
                value: 50.0);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 2,
                columns: new[] { "Number", "Price" },
                values: new object[] { "102", 80.0 });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 3,
                columns: new[] { "Number", "Price", "Status", "Type" },
                values: new object[] { "201", 150.0, "Free", "Suite" });
        }
    }
}
