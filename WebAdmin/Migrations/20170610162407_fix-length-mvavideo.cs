using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAdmin.Migrations
{
    public partial class fixlengthmvavideo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MvaVideos",
                maxLength: 4000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "ntext",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AuthorJobTitle",
                table: "MvaVideos",
                maxLength: 1024,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 384,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Author",
                table: "MvaVideos",
                maxLength: 768,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 384,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MvaVideos",
                type: "ntext",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 4000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AuthorJobTitle",
                table: "MvaVideos",
                maxLength: 384,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 1024,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Author",
                table: "MvaVideos",
                maxLength: 384,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 768,
                oldNullable: true);
        }
    }
}
