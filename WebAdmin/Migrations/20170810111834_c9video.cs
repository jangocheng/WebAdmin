using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAdmin.Migrations
{
    public partial class c9video : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SeriesType",
                table: "C9Videos",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VideoEmbed",
                table: "C9Videos",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_C9Videos_SeriesType",
                table: "C9Videos",
                column: "SeriesType");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_C9Videos_SeriesType",
                table: "C9Videos");

            migrationBuilder.DropColumn(
                name: "SeriesType",
                table: "C9Videos");

            migrationBuilder.DropColumn(
                name: "VideoEmbed",
                table: "C9Videos");
        }
    }
}
