using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace hospitalMng.Migrations
{
    /// <inheritdoc />
    public partial class addRegistration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Users",
                newName: "UserUID");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "Dob",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IllnessDetails",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Mobile",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RelativeName",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RelativeNumber",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Dob",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IllnessDetails",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Mobile",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RelativeName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RelativeNumber",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "UserUID",
                table: "Users",
                newName: "Username");
        }
    }
}
