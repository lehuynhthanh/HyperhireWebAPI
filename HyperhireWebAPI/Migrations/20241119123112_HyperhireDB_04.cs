using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HyperhireWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class HyperhireDB_04 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProductDetail",
                keyColumn: "Id",
                keyValue: new Guid("ef1d0e19-e4de-4fd7-aaa4-c34562759124"));

            migrationBuilder.DeleteData(
                table: "ProductDetail",
                keyColumn: "Id",
                keyValue: new Guid("efccfb09-6827-4537-94a3-ccac017dd414"));

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("bc45ef8a-ddba-4472-b00f-0cbcd61f2ce9"));

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "OrderDetail",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: new Guid("f821cc78-5a56-4b27-9e1c-0582d4f9aaf8"),
                column: "Created",
                value: new DateTime(2024, 11, 19, 19, 31, 11, 884, DateTimeKind.Local).AddTicks(8963));

            migrationBuilder.InsertData(
                table: "ProductDetail",
                columns: new[] { "Id", "CategoryId", "Created", "CreatedBy", "Decription", "ImgUrl", "LastModified", "LastModifiedBy", "Location", "MaxGuests", "MaxNight", "Name", "OriginalPrice", "Price" },
                values: new object[,]
                {
                    { new Guid("bd5886ef-f899-4123-8bf3-e3061100b449"), new Guid("f821cc78-5a56-4b27-9e1c-0582d4f9aaf8"), new DateTime(2024, 11, 19, 19, 31, 11, 884, DateTimeKind.Local).AddTicks(9114), "Admin", "The Mirror Villa is luxurious all the way and features everything you can expect from a smart, upscale property of 21st century. It impresses with utilizing contemporary and distinctive materials, finishing with the utmost attention to details and quality, innovative technologies and high-end appliances.", "[\"img url 1\",\"img url 2\"]", null, null, "Thành phố Nha Trang, Vietnam", 8, 8, "VILLA VENITY Sky", 8m, 6m },
                    { new Guid("c42e227d-7957-4645-bff8-10286a01fb7b"), new Guid("f821cc78-5a56-4b27-9e1c-0582d4f9aaf8"), new DateTime(2024, 11, 19, 19, 31, 11, 884, DateTimeKind.Local).AddTicks(9093), "Admin", "PENHOUSE 180smq 3 with super large PANORAMA BALCONY at CENTANA District 2 HCMC", "[\"img url 1\",\"img url 2\"]", null, null, "Quận 2, Vietnam", 10, 10, "Centana Penthouse 3BR_180sqm Panoramic CityVIEW", 10m, 7m }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "Created", "CreatedBy", "LastModified", "LastModifiedBy", "Password", "UserName" },
                values: new object[] { new Guid("9b268d85-b414-46c6-8cb4-71a4197ddd43"), new DateTime(2024, 11, 19, 19, 31, 11, 884, DateTimeKind.Local).AddTicks(9132), "Admin", null, null, "123456", "123456789" });

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_UserId",
                table: "OrderDetail",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_User_UserId",
                table: "OrderDetail",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_User_UserId",
                table: "OrderDetail");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetail_UserId",
                table: "OrderDetail");

            migrationBuilder.DeleteData(
                table: "ProductDetail",
                keyColumn: "Id",
                keyValue: new Guid("bd5886ef-f899-4123-8bf3-e3061100b449"));

            migrationBuilder.DeleteData(
                table: "ProductDetail",
                keyColumn: "Id",
                keyValue: new Guid("c42e227d-7957-4645-bff8-10286a01fb7b"));

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("9b268d85-b414-46c6-8cb4-71a4197ddd43"));

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "OrderDetail");

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: new Guid("f821cc78-5a56-4b27-9e1c-0582d4f9aaf8"),
                column: "Created",
                value: new DateTime(2024, 11, 19, 15, 49, 28, 5, DateTimeKind.Local).AddTicks(4884));

            migrationBuilder.InsertData(
                table: "ProductDetail",
                columns: new[] { "Id", "CategoryId", "Created", "CreatedBy", "Decription", "ImgUrl", "LastModified", "LastModifiedBy", "Location", "MaxGuests", "MaxNight", "Name", "OriginalPrice", "Price" },
                values: new object[,]
                {
                    { new Guid("ef1d0e19-e4de-4fd7-aaa4-c34562759124"), new Guid("f821cc78-5a56-4b27-9e1c-0582d4f9aaf8"), new DateTime(2024, 11, 19, 15, 49, 28, 5, DateTimeKind.Local).AddTicks(5022), "Admin", "The Mirror Villa is luxurious all the way and features everything you can expect from a smart, upscale property of 21st century. It impresses with utilizing contemporary and distinctive materials, finishing with the utmost attention to details and quality, innovative technologies and high-end appliances.", "[\"img url 1\",\"img url 2\"]", null, null, "Thành phố Nha Trang, Vietnam", 8, 8, "VILLA VENITY Sky", 8m, 6m },
                    { new Guid("efccfb09-6827-4537-94a3-ccac017dd414"), new Guid("f821cc78-5a56-4b27-9e1c-0582d4f9aaf8"), new DateTime(2024, 11, 19, 15, 49, 28, 5, DateTimeKind.Local).AddTicks(5003), "Admin", "PENHOUSE 180smq 3 with super large PANORAMA BALCONY at CENTANA District 2 HCMC", "[\"img url 1\",\"img url 2\"]", null, null, "Quận 2, Vietnam", 10, 10, "Centana Penthouse 3BR_180sqm Panoramic CityVIEW", 10m, 7m }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "Created", "CreatedBy", "LastModified", "LastModifiedBy", "Password", "UserName" },
                values: new object[] { new Guid("bc45ef8a-ddba-4472-b00f-0cbcd61f2ce9"), new DateTime(2024, 11, 19, 15, 49, 28, 5, DateTimeKind.Local).AddTicks(5040), "Admin", null, null, "123456", "Lê Huỳnh Thành" });
        }
    }
}
