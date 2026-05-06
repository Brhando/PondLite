using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PondLite.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddFrogFoundation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Frogs",
                columns: table => new
                {
                    FrogId = table.Column<Guid>(type: "uuid", nullable: false),
                    RelationshipMemberId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CurrentMood = table.Column<string>(type: "text", nullable: false),
                    ActivityState = table.Column<string>(type: "text", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Frogs", x => x.FrogId);
                    table.ForeignKey(
                        name: "FK_Frogs_RelationshipMembers_RelationshipMemberId",
                        column: x => x.RelationshipMemberId,
                        principalTable: "RelationshipMembers",
                        principalColumn: "RelationshipMemberId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Frogs_RelationshipMemberId",
                table: "Frogs",
                column: "RelationshipMemberId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Frogs");
        }
    }
}
