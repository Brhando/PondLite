using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PondLite.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddCancelledAtToRibbits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CancelledAt",
                table: "Ribbits",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CancelledAt",
                table: "Ribbits");
        }
    }
}
