using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WebAdmin.Migrations
{
    public partial class db_relations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BlogId",
                table: "Video",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PracticeId",
                table: "Video",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PracticeId",
                table: "Blog",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Birthday",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.CreateIndex(
                name: "IX_Video_BlogId",
                table: "Video",
                column: "BlogId");

            migrationBuilder.CreateIndex(
                name: "IX_Video_PracticeId",
                table: "Video",
                column: "PracticeId");

            migrationBuilder.CreateIndex(
                name: "IX_Blog_PracticeId",
                table: "Blog",
                column: "PracticeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Blog_Practice_PracticeId",
                table: "Blog",
                column: "PracticeId",
                principalTable: "Practice",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Video_Blog_BlogId",
                table: "Video",
                column: "BlogId",
                principalTable: "Blog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Video_Practice_PracticeId",
                table: "Video",
                column: "PracticeId",
                principalTable: "Practice",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blog_Practice_PracticeId",
                table: "Blog");

            migrationBuilder.DropForeignKey(
                name: "FK_Video_Blog_BlogId",
                table: "Video");

            migrationBuilder.DropForeignKey(
                name: "FK_Video_Practice_PracticeId",
                table: "Video");

            migrationBuilder.DropIndex(
                name: "IX_Video_BlogId",
                table: "Video");

            migrationBuilder.DropIndex(
                name: "IX_Video_PracticeId",
                table: "Video");

            migrationBuilder.DropIndex(
                name: "IX_Blog_PracticeId",
                table: "Blog");

            migrationBuilder.DropColumn(
                name: "BlogId",
                table: "Video");

            migrationBuilder.DropColumn(
                name: "PracticeId",
                table: "Video");

            migrationBuilder.DropColumn(
                name: "PracticeId",
                table: "Blog");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Birthday",
                table: "AspNetUsers",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
