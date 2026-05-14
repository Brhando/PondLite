using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PondLite.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddRibbitFoundation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ribbits",
                columns: table => new
                {
                    RibbitId = table.Column<Guid>(type: "uuid", nullable: false),
                    RelationshipId = table.Column<Guid>(type: "uuid", nullable: false),
                    SenderUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReceiverUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: false),
                    AcknowledgementEmoji = table.Column<string>(type: "text", nullable: true),
                    DeliveryTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AcknowledgedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeclinedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ribbits", x => x.RibbitId);
                    table.ForeignKey(
                        name: "FK_Ribbits_Relationships_RelationshipId",
                        column: x => x.RelationshipId,
                        principalTable: "Relationships",
                        principalColumn: "RelationshipId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ribbits_UserAccounts_ReceiverUserId",
                        column: x => x.ReceiverUserId,
                        principalTable: "UserAccounts",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ribbits_UserAccounts_SenderUserId",
                        column: x => x.SenderUserId,
                        principalTable: "UserAccounts",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ribbits_ReceiverUserId",
                table: "Ribbits",
                column: "ReceiverUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Ribbits_RelationshipId_ReceiverUserId_Status",
                table: "Ribbits",
                columns: new[] { "RelationshipId", "ReceiverUserId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_Ribbits_RelationshipId_SenderUserId_Status",
                table: "Ribbits",
                columns: new[] { "RelationshipId", "SenderUserId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_Ribbits_SenderUserId",
                table: "Ribbits",
                column: "SenderUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ribbits");
        }
    }
}
