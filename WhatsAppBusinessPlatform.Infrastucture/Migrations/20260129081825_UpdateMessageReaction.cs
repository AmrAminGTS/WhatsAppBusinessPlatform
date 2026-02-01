using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WhatsAppBusinessPlatform.Infrastucture.Migrations;

/// <inheritdoc />
public partial class UpdateMessageReaction : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<string>(
            name: "MessageId",
            table: "MessageReaction",
            type: "nvarchar(200)",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)");

        migrationBuilder.CreateIndex(
            name: "IX_MessageReaction_MessageId",
            table: "MessageReaction",
            column: "MessageId",
            unique: true);

        migrationBuilder.AddForeignKey(
            name: "FK_MessageReaction_Messages_MessageId",
            table: "MessageReaction",
            column: "MessageId",
            principalTable: "Messages",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_MessageReaction_Messages_MessageId",
            table: "MessageReaction");

        migrationBuilder.DropIndex(
            name: "IX_MessageReaction_MessageId",
            table: "MessageReaction");

        migrationBuilder.AlterColumn<string>(
            name: "MessageId",
            table: "MessageReaction",
            type: "nvarchar(max)",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(200)");
    }
}
