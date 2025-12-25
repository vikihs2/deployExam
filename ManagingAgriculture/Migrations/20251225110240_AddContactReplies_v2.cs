using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ManagingAgriculture.Migrations
{
    /// <inheritdoc />
    public partial class AddContactReplies_v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsReplied",
                table: "ContactForms",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "RepliedBy",
                table: "ContactForms",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RepliedDate",
                table: "ContactForms",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReplyMessage",
                table: "ContactForms",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReplied",
                table: "ContactForms");

            migrationBuilder.DropColumn(
                name: "RepliedBy",
                table: "ContactForms");

            migrationBuilder.DropColumn(
                name: "RepliedDate",
                table: "ContactForms");

            migrationBuilder.DropColumn(
                name: "ReplyMessage",
                table: "ContactForms");
        }
    }
}
