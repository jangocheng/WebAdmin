using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WebAdmin.Migrations
{
    public partial class _0607 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "C9Videos",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Author = table.Column<string>(maxLength: 64, nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(maxLength: 4096, nullable: true),
                    Duration = table.Column<string>(maxLength: 32, nullable: true),
                    Language = table.Column<string>(maxLength: 16, nullable: true),
                    Mp3Url = table.Column<string>(maxLength: 128, nullable: true),
                    Mp4HigUrl = table.Column<string>(maxLength: 128, nullable: true),
                    Mp4LowUrl = table.Column<string>(maxLength: 128, nullable: true),
                    Mp4MidUrl = table.Column<string>(maxLength: 128, nullable: true),
                    SeriesTitle = table.Column<string>(maxLength: 128, nullable: true),
                    SeriesTitleUrl = table.Column<string>(maxLength: 128, nullable: true),
                    SourceUrl = table.Column<string>(maxLength: 128, nullable: true),
                    Status = table.Column<int>(nullable: true),
                    Tags = table.Column<string>(maxLength: 128, nullable: true),
                    ThumbnailUrl = table.Column<string>(maxLength: 256, nullable: true),
                    Title = table.Column<string>(maxLength: 256, nullable: true),
                    UpdatedTime = table.Column<DateTime>(nullable: false),
                    Views = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_C9Videos", x => x.Id);
                });



            migrationBuilder.CreateIndex(
                name: "IX_BingNews_UpdatedTime",
                table: "BingNews",
                column: "UpdatedTime");

            migrationBuilder.CreateIndex(
                name: "IX_C9Articles_SeriesTitle",
                table: "C9Articles",
                column: "SeriesTitle");

            migrationBuilder.CreateIndex(
                name: "IX_C9Articles_Title",
                table: "C9Articles",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_C9Articles_UpdatedTime",
                table: "C9Articles",
                column: "UpdatedTime");

            migrationBuilder.CreateIndex(
                name: "IX_C9Videos_Language",
                table: "C9Videos",
                column: "Language");

            migrationBuilder.CreateIndex(
                name: "IX_C9Videos_SeriesTitle",
                table: "C9Videos",
                column: "SeriesTitle");

            migrationBuilder.CreateIndex(
                name: "IX_C9Videos_Title",
                table: "C9Videos",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_C9Videos_UpdatedTime",
                table: "C9Videos",
                column: "UpdatedTime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "C9Videos");
        }
    }
}
