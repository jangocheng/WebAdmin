using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WebAdmin.Migrations
{
    public partial class activity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blog_Practice_PracticeId",
                table: "Blog");

            migrationBuilder.DropIndex(
                name: "IX_Blog_PracticeId",
                table: "Blog");

            migrationBuilder.DropColumn(
                name: "PracticeId",
                table: "Blog");

            migrationBuilder.AddColumn<Guid>(
                name: "BlogId",
                table: "Practice",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Activity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActivityType = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Content = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CurrentPeopleNumber = table.Column<int>(type: "int", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaxPeopleNumber = table.Column<int>(type: "int", nullable: false),
                    OnlineUrl = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    OrganizerId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    ThumbnailUrl = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    UpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Views = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Activity_AspNetUsers_OrganizerId",
                        column: x => x.OrganizerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserActivity",
                columns: table => new
                {
                    AcitvityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserActivity", x => new { x.AcitvityId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserActivity_Activity_AcitvityId",
                        column: x => x.AcitvityId,
                        principalTable: "Activity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserActivity_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Practice_BlogId",
                table: "Practice",
                column: "BlogId",
                unique: true,
                filter: "[BlogId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Activity_OrganizerId",
                table: "Activity",
                column: "OrganizerId");

            migrationBuilder.CreateIndex(
                name: "IX_UserActivity_UserId",
                table: "UserActivity",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Practice_Blog_BlogId",
                table: "Practice",
                column: "BlogId",
                principalTable: "Blog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Practice_Blog_BlogId",
                table: "Practice");

            migrationBuilder.DropTable(
                name: "UserActivity");

            migrationBuilder.DropTable(
                name: "Activity");

            migrationBuilder.DropIndex(
                name: "IX_Practice_BlogId",
                table: "Practice");

            migrationBuilder.DropColumn(
                name: "BlogId",
                table: "Practice");

            migrationBuilder.AddColumn<Guid>(
                name: "PracticeId",
                table: "Blog",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Blog_PracticeId",
                table: "Blog",
                column: "PracticeId",
                unique: true,
                filter: "[PracticeId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Blog_Practice_PracticeId",
                table: "Blog",
                column: "PracticeId",
                principalTable: "Practice",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
