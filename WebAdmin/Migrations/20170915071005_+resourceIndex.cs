using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WebAdmin.Migrations
{
    public partial class resourceIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Resource_Name",
                table: "Resource",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Resource_Tag",
                table: "Resource",
                column: "Tag");

            migrationBuilder.CreateIndex(
                name: "IX_Resource_Type",
                table: "Resource",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_Resource_UpdatedTime",
                table: "Resource",
                column: "UpdatedTime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Resource_Name",
                table: "Resource");

            migrationBuilder.DropIndex(
                name: "IX_Resource_Tag",
                table: "Resource");

            migrationBuilder.DropIndex(
                name: "IX_Resource_Type",
                table: "Resource");

            migrationBuilder.DropIndex(
                name: "IX_Resource_UpdatedTime",
                table: "Resource");
        }
    }
}
