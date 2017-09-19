using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WebAdmin.Migrations
{
    public partial class addIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MvaVideos_Technologies",
                table: "MvaVideos");

            migrationBuilder.CreateIndex(
                name: "IX_RssNews_Categories",
                table: "RssNews",
                column: "Categories");

            migrationBuilder.CreateIndex(
                name: "IX_RssNews_LastUpdateTime",
                table: "RssNews",
                column: "LastUpdateTime");

            migrationBuilder.CreateIndex(
                name: "IX_RssNews_Title",
                table: "RssNews",
                column: "Title",
                unique: true,
                filter: "[Title] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_MvaVideos_Technologies",
                table: "MvaVideos",
                column: "Tags");

            migrationBuilder.CreateIndex(
                name: "IX_C9Videos_Tags",
                table: "C9Videos",
                column: "Tags");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RssNews_Categories",
                table: "RssNews");

            migrationBuilder.DropIndex(
                name: "IX_RssNews_LastUpdateTime",
                table: "RssNews");

            migrationBuilder.DropIndex(
                name: "IX_RssNews_Title",
                table: "RssNews");

            migrationBuilder.DropIndex(
                name: "IX_MvaVideos_Technologies",
                table: "MvaVideos");

            migrationBuilder.DropIndex(
                name: "IX_C9Videos_Tags",
                table: "C9Videos");

            migrationBuilder.CreateIndex(
                name: "IX_MvaVideos_Technologies",
                table: "MvaVideos",
                column: "Technologies");
        }
    }
}
