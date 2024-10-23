using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkylineRealty.API.Migrations
{
    public partial class UpdatePropertyTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Adress",
                table: "Properties",
                newName: "Address");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 23, 15, 59, 2, 431, DateTimeKind.Local).AddTicks(9760));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 23, 15, 59, 2, 431, DateTimeKind.Local).AddTicks(9775));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 23, 15, 59, 2, 431, DateTimeKind.Local).AddTicks(9776));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Properties",
                newName: "Adress");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 23, 15, 53, 4, 79, DateTimeKind.Local).AddTicks(7223));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 23, 15, 53, 4, 79, DateTimeKind.Local).AddTicks(7243));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 23, 15, 53, 4, 79, DateTimeKind.Local).AddTicks(7245));
        }
    }
}
