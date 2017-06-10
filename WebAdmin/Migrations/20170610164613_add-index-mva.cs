using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAdmin.Migrations
{
    public partial class addindexmva : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_MvaVideos_LanguageCode",
                table: "MvaVideos",
                column: "LanguageCode");

            migrationBuilder.CreateIndex(
                name: "IX_MvaVideos_Title",
                table: "MvaVideos",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_MvaVideos_UpdatedTime",
                table: "MvaVideos",
                column: "UpdatedTime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MvaVideos_LanguageCode",
                table: "MvaVideos");

            migrationBuilder.DropIndex(
                name: "IX_MvaVideos_Title",
                table: "MvaVideos");

            migrationBuilder.DropIndex(
                name: "IX_MvaVideos_UpdatedTime",
                table: "MvaVideos");
        }
    }
}
