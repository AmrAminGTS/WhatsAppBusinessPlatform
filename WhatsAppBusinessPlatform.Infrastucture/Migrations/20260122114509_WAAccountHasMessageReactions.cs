using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WhatsAppBusinessPlatform.Infrastucture.Migrations;

/// <inheritdoc />
public partial class WAAccountHasMessageReactions : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_MessageReaction_Accounts_ReactedByAccountId",
            table: "MessageReaction");

        migrationBuilder.AddForeignKey(
            name: "FK_MessageReaction_Accounts_ReactedByAccountId",
            table: "MessageReaction",
            column: "ReactedByAccountId",
            principalTable: "Accounts",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_MessageReaction_Accounts_ReactedByAccountId",
            table: "MessageReaction");

        migrationBuilder.AddForeignKey(
            name: "FK_MessageReaction_Accounts_ReactedByAccountId",
            table: "MessageReaction",
            column: "ReactedByAccountId",
            principalTable: "Accounts",
            principalColumn: "Id");
    }
}
