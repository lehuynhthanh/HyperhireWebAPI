using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HyperhireWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class HyperhireDB_02 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Category",
                columns: new[] { "Id", "CategoryNameId", "Created", "CreatedBy", "Icon", "IsActive", "LastModified", "LastModifiedBy", "Name" },
                values: new object[] { new Guid("f821cc78-5a56-4b27-9e1c-0582d4f9aaf8"), "amazing_view", new DateTime(2024, 11, 19, 10, 52, 52, 542, DateTimeKind.Local).AddTicks(6262), "Admin", "url icon", true, null, null, "Amazing View" });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "Created", "CreatedBy", "LastModified", "LastModifiedBy", "Password", "UserName" },
                values: new object[] { new Guid("6d09ce62-377f-4627-bb91-d7d0e8a339e4"), new DateTime(2024, 11, 19, 10, 52, 52, 542, DateTimeKind.Local).AddTicks(6423), "Admin", null, null, "123456", "Lê Huỳnh Thành" });

            migrationBuilder.InsertData(
                table: "ProductDetail",
                columns: new[] { "Id", "CategoryId", "Created", "CreatedBy", "Decription", "ImgUrl", "LastModified", "LastModifiedBy", "Location", "MaxGuests", "MaxNight", "Name", "OriginalPrice", "Price" },
                values: new object[,]
                {
                    { new Guid("13bb2fc1-d8b8-4183-856e-83d7673b6350"), new Guid("f821cc78-5a56-4b27-9e1c-0582d4f9aaf8"), new DateTime(2024, 11, 19, 10, 52, 52, 542, DateTimeKind.Local).AddTicks(6385), "Admin", "PENHOUSE 180smq 3 with super large PANORAMA BALCONY at CENTANA District 2 HCMC", "[\"img url 1\",\"img url 2\"]", null, null, "Quận 2, Vietnam", 10, 10, "Centana Penthouse 3BR_180sqm Panoramic CityVIEW", 10m, 7m },
                    { new Guid("7a78917f-5362-47f5-a60e-74476fb8a2f0"), new Guid("f821cc78-5a56-4b27-9e1c-0582d4f9aaf8"), new DateTime(2024, 11, 19, 10, 52, 52, 542, DateTimeKind.Local).AddTicks(6406), "Admin", "The Mirror Villa is luxurious all the way and features everything you can expect from a smart, upscale property of 21st century. It impresses with utilizing contemporary and distinctive materials, finishing with the utmost attention to details and quality, innovative technologies and high-end appliances.", "[\"img url 1\",\"img url 2\"]", null, null, "Thành phố Nha Trang, Vietnam", 8, 8, "VILLA VENITY Sky", 8m, 6m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProductDetail",
                keyColumn: "Id",
                keyValue: new Guid("13bb2fc1-d8b8-4183-856e-83d7673b6350"));

            migrationBuilder.DeleteData(
                table: "ProductDetail",
                keyColumn: "Id",
                keyValue: new Guid("7a78917f-5362-47f5-a60e-74476fb8a2f0"));

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("6d09ce62-377f-4627-bb91-d7d0e8a339e4"));

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: new Guid("f821cc78-5a56-4b27-9e1c-0582d4f9aaf8"));
        }
    }
}
