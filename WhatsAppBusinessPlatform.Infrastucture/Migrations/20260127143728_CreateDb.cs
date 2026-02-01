using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WhatsAppBusinessPlatform.Infrastucture.Migrations;

/// <inheritdoc />
public partial class CreateDb : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Accounts",
            columns: table => new
            {
                Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                PhoneNumber = table.Column<string>(type: "nvarchar(49)", maxLength: 49, nullable: false),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Accounts", x => x.Id);
                table.UniqueConstraint("AK_Accounts_PhoneNumber", x => x.PhoneNumber);
            });

        migrationBuilder.CreateTable(
            name: "Contacts",
            columns: table => new
            {
                Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                WAAcountId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Contacts", x => x.Id);
                table.ForeignKey(
                    name: "FK_Contacts_Accounts_WAAcountId",
                    column: x => x.WAAcountId,
                    principalTable: "Accounts",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "Messages",
            columns: table => new
            {
                Id = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                DateTimeOffset = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                ContentType = table.Column<int>(type: "int", nullable: false),
                JsonContent = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                Direction = table.Column<int>(type: "int", nullable: false),
                BusinessPhoneId = table.Column<string>(type: "nvarchar(49)", maxLength: 49, nullable: false),
                ContactPhoneNumber = table.Column<string>(type: "nvarchar(49)", maxLength: 49, nullable: false),
                RecipientType = table.Column<int>(type: "int", nullable: false),
                ReplyToId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                RawWebhookReqest = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                CreatedByUserId = table.Column<string>(type: "nvarchar(199)", maxLength: 199, nullable: true),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Messages", x => x.Id);
                table.ForeignKey(
                    name: "FK_Messages_Accounts_ContactPhoneNumber",
                    column: x => x.ContactPhoneNumber,
                    principalTable: "Accounts",
                    principalColumn: "PhoneNumber",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_Messages_Messages_ReplyToId",
                    column: x => x.ReplyToId,
                    principalTable: "Messages",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "MessageReaction",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", maxLength: 200, nullable: false),
                Emoji = table.Column<string>(type: "nvarchar(49)", maxLength: 49, nullable: false),
                DateTimeOffset = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                Direction = table.Column<int>(type: "int", nullable: false),
                MessageId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ReactedToMessageId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                UserId = table.Column<string>(type: "nvarchar(199)", maxLength: 199, nullable: true),
                ContactAccountId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MessageReaction", x => x.Id);
                table.ForeignKey(
                    name: "FK_MessageReaction_Accounts_ContactAccountId",
                    column: x => x.ContactAccountId,
                    principalTable: "Accounts",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_MessageReaction_Messages_ReactedToMessageId",
                    column: x => x.ReactedToMessageId,
                    principalTable: "Messages",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "MessageReader",
            columns: table => new
            {
                UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                MessageId = table.Column<string>(type: "nvarchar(200)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MessageReader", x => new { x.UserId, x.MessageId });
                table.ForeignKey(
                    name: "FK_MessageReader_Messages_MessageId",
                    column: x => x.MessageId,
                    principalTable: "Messages",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "MessageStatuses",
            columns: table => new
            {
                Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                DateTimeOffset = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                Status = table.Column<int>(type: "int", nullable: false),
                IsBillable = table.Column<bool>(type: "bit", nullable: false),
                PricingModel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                PricingCategory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                PricingType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                RawWebhookReqest = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                MessageId = table.Column<string>(type: "nvarchar(200)", nullable: false),
                MessageReactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MessageStatuses", x => x.Id);
                table.ForeignKey(
                    name: "FK_MessageStatuses_MessageReaction_MessageReactionId",
                    column: x => x.MessageReactionId,
                    principalTable: "MessageReaction",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_MessageStatuses_Messages_MessageId",
                    column: x => x.MessageId,
                    principalTable: "Messages",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "StatusError",
            columns: table => new
            {
                Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                Code = table.Column<int>(type: "int", nullable: false),
                Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Details = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Href = table.Column<string>(type: "nvarchar(max)", nullable: false),
                StatusId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_StatusError", x => x.Id);
                table.ForeignKey(
                    name: "FK_StatusError_MessageStatuses_StatusId",
                    column: x => x.StatusId,
                    principalTable: "MessageStatuses",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Accounts_PhoneNumber",
            table: "Accounts",
            column: "PhoneNumber",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Contacts_WAAcountId",
            table: "Contacts",
            column: "WAAcountId",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_MessageReaction_ContactAccountId",
            table: "MessageReaction",
            column: "ContactAccountId");

        migrationBuilder.CreateIndex(
            name: "IX_MessageReaction_ReactedToMessageId",
            table: "MessageReaction",
            column: "ReactedToMessageId");

        migrationBuilder.CreateIndex(
            name: "IX_MessageReader_MessageId",
            table: "MessageReader",
            column: "MessageId");

        migrationBuilder.CreateIndex(
            name: "IX_Messages_ContactPhoneNumber",
            table: "Messages",
            column: "ContactPhoneNumber");

        migrationBuilder.CreateIndex(
            name: "IX_Messages_ReplyToId",
            table: "Messages",
            column: "ReplyToId");

        migrationBuilder.CreateIndex(
            name: "IX_MessageStatuses_MessageId",
            table: "MessageStatuses",
            column: "MessageId");

        migrationBuilder.CreateIndex(
            name: "IX_MessageStatuses_MessageReactionId",
            table: "MessageStatuses",
            column: "MessageReactionId");

        migrationBuilder.CreateIndex(
            name: "IX_StatusError_StatusId",
            table: "StatusError",
            column: "StatusId",
            unique: true,
            filter: "[StatusId] IS NOT NULL");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Contacts");

        migrationBuilder.DropTable(
            name: "MessageReader");

        migrationBuilder.DropTable(
            name: "StatusError");

        migrationBuilder.DropTable(
            name: "MessageStatuses");

        migrationBuilder.DropTable(
            name: "MessageReaction");

        migrationBuilder.DropTable(
            name: "Messages");

        migrationBuilder.DropTable(
            name: "Accounts");
    }
}
