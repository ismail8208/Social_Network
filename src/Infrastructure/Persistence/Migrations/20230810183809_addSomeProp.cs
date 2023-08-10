using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediaLink.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addSomeProp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WorkEnvironment",
                table: "Jobs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WorkSchedule",
                table: "Jobs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "Experiences",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartedTime",
                table: "Experiences",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkEnvironment",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "WorkSchedule",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "Experiences");

            migrationBuilder.DropColumn(
                name: "StartedTime",
                table: "Experiences");
        }
    }
}
