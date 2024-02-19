using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commers.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "_roles",
                table: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid[]>(
                name: "_roles",
                table: "Users",
                type: "uuid[]",
                nullable: false,
                defaultValue: new Guid[0]);
        }
    }
}
