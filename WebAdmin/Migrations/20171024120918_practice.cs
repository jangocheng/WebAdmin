using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WebAdmin.Migrations
{
    public partial class practice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PracticeId",
                table: "Blog",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "VideoId",
                table: "Blog",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Practice",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    UpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VideoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Practice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Practice_Video_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Video",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Blog_PracticeId",
                table: "Blog",
                column: "PracticeId",
                unique: true,
                filter: "[PracticeId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Blog_VideoId",
                table: "Blog",
                column: "VideoId",
                unique: true,
                filter: "[VideoId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Practice_VideoId",
                table: "Practice",
                column: "VideoId",
                unique: true,
                filter: "[VideoId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Blog_Practice_PracticeId",
                table: "Blog",
                column: "PracticeId",
                principalTable: "Practice",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Blog_Video_VideoId",
                table: "Blog",
                column: "VideoId",
                principalTable: "Video",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blog_Practice_PracticeId",
                table: "Blog");

            migrationBuilder.DropForeignKey(
                name: "FK_Blog_Video_VideoId",
                table: "Blog");

            migrationBuilder.DropTable(
                name: "Practice");

            migrationBuilder.DropIndex(
                name: "IX_Blog_PracticeId",
                table: "Blog");

            migrationBuilder.DropIndex(
                name: "IX_Blog_VideoId",
                table: "Blog");

            migrationBuilder.DropColumn(
                name: "PracticeId",
                table: "Blog");

            migrationBuilder.DropColumn(
                name: "VideoId",
                table: "Blog");
        }
    }
}
