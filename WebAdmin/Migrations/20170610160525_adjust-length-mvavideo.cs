using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAdmin.Migrations
{
    public partial class adjustlengthmvavideo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Technologies",
                table: "MvaVideos",
                maxLength: 384,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Tags",
                table: "MvaVideos",
                maxLength: 384,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MvaVideos",
                type: "ntext",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 4000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CourseNumber",
                table: "MvaVideos",
                maxLength: 128,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AuthorJobTitle",
                table: "MvaVideos",
                maxLength: 384,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AuthorCompany",
                table: "MvaVideos",
                maxLength: 384,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Author",
                table: "MvaVideos",
                maxLength: 384,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Technologies",
                table: "MvaVideos",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 384,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Tags",
                table: "MvaVideos",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 384,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MvaVideos",
                maxLength: 4000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "ntext",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CourseNumber",
                table: "MvaVideos",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 128,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AuthorJobTitle",
                table: "MvaVideos",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 384,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AuthorCompany",
                table: "MvaVideos",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 384,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Author",
                table: "MvaVideos",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 384,
                oldNullable: true);
        }
    }
}
