using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ManagingAgriculture.Migrations
{
    /// <inheritdoc />
    public partial class AddCompanyStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Sensors",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OwnerUserId",
                table: "Sensors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Resources",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OwnerUserId",
                table: "Resources",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Plants",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OwnerUserId",
                table: "Plants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Machinery",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OwnerUserId",
                table: "Machinery",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sensors_CompanyId",
                table: "Sensors",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Resources_CompanyId",
                table: "Resources",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Plants_CompanyId",
                table: "Plants",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Machinery_CompanyId",
                table: "Machinery",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CompanyId",
                table: "AspNetUsers",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Companies_CompanyId",
                table: "AspNetUsers",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Machinery_Companies_CompanyId",
                table: "Machinery",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Plants_Companies_CompanyId",
                table: "Plants",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Resources_Companies_CompanyId",
                table: "Resources",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sensors_Companies_CompanyId",
                table: "Sensors",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Companies_CompanyId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Machinery_Companies_CompanyId",
                table: "Machinery");

            migrationBuilder.DropForeignKey(
                name: "FK_Plants_Companies_CompanyId",
                table: "Plants");

            migrationBuilder.DropForeignKey(
                name: "FK_Resources_Companies_CompanyId",
                table: "Resources");

            migrationBuilder.DropForeignKey(
                name: "FK_Sensors_Companies_CompanyId",
                table: "Sensors");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_Sensors_CompanyId",
                table: "Sensors");

            migrationBuilder.DropIndex(
                name: "IX_Resources_CompanyId",
                table: "Resources");

            migrationBuilder.DropIndex(
                name: "IX_Plants_CompanyId",
                table: "Plants");

            migrationBuilder.DropIndex(
                name: "IX_Machinery_CompanyId",
                table: "Machinery");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CompanyId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Sensors");

            migrationBuilder.DropColumn(
                name: "OwnerUserId",
                table: "Sensors");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Resources");

            migrationBuilder.DropColumn(
                name: "OwnerUserId",
                table: "Resources");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Plants");

            migrationBuilder.DropColumn(
                name: "OwnerUserId",
                table: "Plants");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Machinery");

            migrationBuilder.DropColumn(
                name: "OwnerUserId",
                table: "Machinery");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");
        }
    }
}
