using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PondLite.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddDiscussionPrompts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DiscussionPrompts",
                columns: table => new
                {
                    DiscussionPromptId = table.Column<Guid>(type: "uuid", nullable: false),
                    RelationshipId = table.Column<Guid>(type: "uuid", nullable: false),
                    PromptText = table.Column<string>(type: "text", nullable: false),
                    PromptDate = table.Column<DateOnly>(type: "date", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscussionPrompts", x => x.DiscussionPromptId);
                    table.ForeignKey(
                        name: "FK_DiscussionPrompts_Relationships_RelationshipId",
                        column: x => x.RelationshipId,
                        principalTable: "Relationships",
                        principalColumn: "RelationshipId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DiscussionResponses",
                columns: table => new
                {
                    DiscussionResponseId = table.Column<Guid>(type: "uuid", nullable: false),
                    DiscussionPromptId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserAccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    ResponseText = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscussionResponses", x => x.DiscussionResponseId);
                    table.ForeignKey(
                        name: "FK_DiscussionResponses_DiscussionPrompts_DiscussionPromptId",
                        column: x => x.DiscussionPromptId,
                        principalTable: "DiscussionPrompts",
                        principalColumn: "DiscussionPromptId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DiscussionResponses_UserAccounts_UserAccountId",
                        column: x => x.UserAccountId,
                        principalTable: "UserAccounts",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiscussionPrompts_RelationshipId_PromptDate",
                table: "DiscussionPrompts",
                columns: new[] { "RelationshipId", "PromptDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DiscussionResponses_DiscussionPromptId_UserAccountId",
                table: "DiscussionResponses",
                columns: new[] { "DiscussionPromptId", "UserAccountId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DiscussionResponses_UserAccountId",
                table: "DiscussionResponses",
                column: "UserAccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiscussionResponses");

            migrationBuilder.DropTable(
                name: "DiscussionPrompts");
        }
    }
}
