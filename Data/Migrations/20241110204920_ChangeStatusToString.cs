using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymMemberships.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeStatusToString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Members",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Members",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Members",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Members",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Members");
        }
    }
}
