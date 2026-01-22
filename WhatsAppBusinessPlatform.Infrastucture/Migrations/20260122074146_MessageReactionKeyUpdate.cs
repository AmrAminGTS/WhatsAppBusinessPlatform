using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WhatsAppBusinessPlatform.Infrastucture.Migrations;

/// <inheritdoc />
public partial class MessageReactionKeyUpdate : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_MessageReaction_Accounts_ReactedByAccountId",
            table: "MessageReaction");

        migrationBuilder.DropPrimaryKey(
            name: "PK_MessageReaction",
            table: "MessageReaction");

        migrationBuilder.AlterColumn<string>(
            name: "ReactedByAccountId",
            table: "MessageReaction",
            type: "nvarchar(450)",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(450)");

        migrationBuilder.AddColumn<long>(
            name: "Id",
            table: "MessageReaction",
            type: "bigint",
            nullable: false,
            defaultValue: 0L)
            .Annotation("SqlServer:Identity", "1, 1");

        migrationBuilder.AddPrimaryKey(
            name: "PK_MessageReaction",
            table: "MessageReaction",
            column: "Id");

        migrationBuilder.CreateIndex(
            name: "IX_MessageReaction_ReactedToMessageId",
            table: "MessageReaction",
            column: "ReactedToMessageId");

        migrationBuilder.AddForeignKey(
            name: "FK_MessageReaction_Accounts_ReactedByAccountId",
            table: "MessageReaction",
            column: "ReactedByAccountId",
            principalTable: "Accounts",
            principalColumn: "Id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_MessageReaction_Accounts_ReactedByAccountId",
            table: "MessageReaction");

        migrationBuilder.DropPrimaryKey(
            name: "PK_MessageReaction",
            table: "MessageReaction");

        migrationBuilder.DropIndex(
            name: "IX_MessageReaction_ReactedToMessageId",
            table: "MessageReaction");

        migrationBuilder.DropColumn(
            name: "Id",
            table: "MessageReaction");

        migrationBuilder.AlterColumn<string>(
            name: "ReactedByAccountId",
            table: "MessageReaction",
            type: "nvarchar(450)",
            nullable: false,
            defaultValue: "",
            oldClrType: typeof(string),
            oldType: "nvarchar(450)",
            oldNullable: true);

        migrationBuilder.AddPrimaryKey(
            name: "PK_MessageReaction",
            table: "MessageReaction",
            columns: ["ReactedToMessageId", "Emoji", "Direction"]);

        migrationBuilder.AddForeignKey(
            name: "FK_MessageReaction_Accounts_ReactedByAccountId",
            table: "MessageReaction",
            column: "ReactedByAccountId",
            principalTable: "Accounts",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }
}
