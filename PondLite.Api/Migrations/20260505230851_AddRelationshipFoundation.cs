using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PondLite.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationshipFoundation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Relationships",
                columns: table => new
                {
                    RelationshipId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Relationships", x => x.RelationshipId);
                });

            migrationBuilder.CreateTable(
                name: "RelationshipMembers",
                columns: table => new
                {
                    RelationshipMemberId = table.Column<Guid>(type: "uuid", nullable: false),
                    RelationshipId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserAccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    DisplayName = table.Column<string>(type: "text", nullable: false),
                    Role = table.Column<string>(type: "text", nullable: false),
                    JoinedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelationshipMembers", x => x.RelationshipMemberId);
                    table.ForeignKey(
                        name: "FK_RelationshipMembers_Relationships_RelationshipId",
                        column: x => x.RelationshipId,
                        principalTable: "Relationships",
                        principalColumn: "RelationshipId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RelationshipMembers_UserAccounts_UserAccountId",
                        column: x => x.UserAccountId,
                        principalTable: "UserAccounts",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthTokens_UserAccountId",
                table: "AuthTokens",
                column: "UserAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_RelationshipMembers_RelationshipId_UserAccountId",
                table: "RelationshipMembers",
                columns: new[] { "RelationshipId", "UserAccountId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RelationshipMembers_UserAccountId",
                table: "RelationshipMembers",
                column: "UserAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuthTokens_UserAccounts_UserAccountId",
                table: "AuthTokens",
                column: "UserAccountId",
                principalTable: "UserAccounts",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthTokens_UserAccounts_UserAccountId",
                table: "AuthTokens");

            migrationBuilder.DropTable(
                name: "RelationshipMembers");

            migrationBuilder.DropTable(
                name: "Relationships");

            migrationBuilder.DropIndex(
                name: "IX_AuthTokens_UserAccountId",
                table: "AuthTokens");
        }
    }
}
