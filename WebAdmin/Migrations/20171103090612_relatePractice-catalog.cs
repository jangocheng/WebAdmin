using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WebAdmin.Migrations
{
    public partial class relatePracticecatalog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CatalogId",
                table: "Practice",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Practice_CatalogId",
                table: "Practice",
                column: "CatalogId");

            migrationBuilder.AddForeignKey(
                name: "FK_Practice_Catalog_CatalogId",
                table: "Practice",
                column: "CatalogId",
                principalTable: "Catalog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Practice_Catalog_CatalogId",
                table: "Practice");

            migrationBuilder.DropIndex(
                name: "IX_Practice_CatalogId",
                table: "Practice");

            migrationBuilder.DropColumn(
                name: "CatalogId",
                table: "Practice");
        }
    }
}
