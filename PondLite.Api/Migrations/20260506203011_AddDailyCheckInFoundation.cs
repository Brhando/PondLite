using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PondLite.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddDailyCheckInFoundation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccountType",
                table: "UserAccounts",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "DailyCheckIns",
                columns: table => new
                {
                    DailyCheckInId = table.Column<Guid>(type: "uuid", nullable: false),
                    RelationshipId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserAccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    CheckInDate = table.Column<DateOnly>(type: "date", nullable: false),
                    PrimaryEmotion = table.Column<string>(type: "text", nullable: false),
                    SecondaryEmotion = table.Column<string>(type: "text", nullable: true),
                    TertiaryEmotion = table.Column<string>(type: "text", nullable: true),
                    OptionalMessage = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyCheckIns", x => x.DailyCheckInId);
                    table.ForeignKey(
                        name: "FK_DailyCheckIns_Relationships_RelationshipId",
                        column: x => x.RelationshipId,
                        principalTable: "Relationships",
                        principalColumn: "RelationshipId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DailyCheckIns_UserAccounts_UserAccountId",
                        column: x => x.UserAccountId,
                        principalTable: "UserAccounts",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DailyCheckIns_RelationshipId_UserAccountId_CheckInDate",
                table: "DailyCheckIns",
                columns: new[] { "RelationshipId", "UserAccountId", "CheckInDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DailyCheckIns_UserAccountId",
                table: "DailyCheckIns",
                column: "UserAccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DailyCheckIns");

            migrationBuilder.DropColumn(
                name: "AccountType",
                table: "UserAccounts");
        }
    }
}
