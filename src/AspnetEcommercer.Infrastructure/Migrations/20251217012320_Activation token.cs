using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspnetEcommerce.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Activationtoken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomerActivationTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Token = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ExpiresAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UsedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerActivationTokens", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerActivationTokens_CustomerId",
                table: "CustomerActivationTokens",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerActivationTokens_Token",
                table: "CustomerActivationTokens",
                column: "Token",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerActivationTokens");
        }
    }
}
