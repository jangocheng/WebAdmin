using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAdmin.Migrations
{
    public partial class mvaVideo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SeriesTitle",
                table: "C9Videos",
                maxLength: 512,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "MvaVideos",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Author = table.Column<string>(maxLength: 256, nullable: true),
                    AuthorCompany = table.Column<string>(maxLength: 256, nullable: true),
                    AuthorJobTitle = table.Column<string>(maxLength: 256, nullable: true),
                    CourseDuration = table.Column<string>(maxLength: 32, nullable: true),
                    CourseImage = table.Column<string>(maxLength: 512, nullable: true),
                    CourseLevel = table.Column<string>(maxLength: 32, nullable: true),
                    CourseNumber = table.Column<string>(maxLength: 256, nullable: true),
                    CourseStatus = table.Column<string>(maxLength: 32, nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(maxLength: 4000, nullable: true),
                    LanguageCode = table.Column<string>(maxLength: 16, nullable: true),
                    MvaId = table.Column<int>(nullable: true),
                    ProductPackageVersionId = table.Column<int>(nullable: true),
                    SourceUrl = table.Column<string>(maxLength: 512, nullable: true),
                    Status = table.Column<int>(nullable: true),
                    Tags = table.Column<string>(maxLength: 256, nullable: true),
                    Technologies = table.Column<string>(maxLength: 256, nullable: true),
                    Title = table.Column<string>(maxLength: 256, nullable: true),
                    UpdatedTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MvaVideos", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MvaVideos");

            migrationBuilder.AlterColumn<string>(
                name: "SeriesTitle",
                table: "C9Videos",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 512,
                oldNullable: true);
        }
    }
}
