using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymMangementDAL.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateHealtgRecordTableAndGymUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Width",
                table: "Members",
                newName: "Weight");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "DateOfBirth",
                table: "Trainers",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "DateOfBirth",
                table: "Members",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Weight",
                table: "Members",
                newName: "Width");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfBirth",
                table: "Trainers",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfBirth",
                table: "Members",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");
        }
    }
}
