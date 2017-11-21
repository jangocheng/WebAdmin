using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WebAdmin.Migrations.Community
{
    public partial class CommunityBlog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Config");

            migrationBuilder.CreateTable(
                name: "LikeAnalysis",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AgreeNumber = table.Column<long>(type: "int8", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "timestamp", nullable: false),
                    LikeNumber = table.Column<long>(type: "int8", nullable: false),
                    ObjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    ObjectType = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true),
                    UnLinkeNumber = table.Column<long>(type: "int8", nullable: false),
                    UpdatedTime = table.Column<DateTime>(type: "timestamp", nullable: false),
                    ViewNumber = table.Column<long>(type: "int8", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LikeAnalysis", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Series",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CatalogName = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "timestamp", nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    Status = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true),
                    UpdatedTime = table.Column<DateTime>(type: "timestamp", nullable: false),
                    Value = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Series", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Blog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AuthorId = table.Column<Guid>(type: "uuid", nullable: false),
                    AuthorName = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true),
                    CatalogName = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true),
                    Content = table.Column<string>(type: "text", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "timestamp", nullable: false),
                    Description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    IsRecommend = table.Column<bool>(type: "bool", nullable: false),
                    SeriesId = table.Column<Guid>(type: "uuid", nullable: false),
                    SourceUrl = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    Status = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true),
                    Tags = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    Title = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true),
                    Type = table.Column<string>(type: "text", nullable: true),
                    UpdatedTime = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Blog_Series_SeriesId",
                        column: x => x.SeriesId,
                        principalTable: "Series",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Blog_CatalogName",
                table: "Blog",
                column: "CatalogName");

            migrationBuilder.CreateIndex(
                name: "IX_Blog_SeriesId",
                table: "Blog",
                column: "SeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_Blog_UpdatedTime",
                table: "Blog",
                column: "UpdatedTime");

            migrationBuilder.CreateIndex(
                name: "IX_LikeAnalysis_ObjectId",
                table: "LikeAnalysis",
                column: "ObjectId");

            migrationBuilder.CreateIndex(
                name: "IX_LikeAnalysis_ObjectType",
                table: "LikeAnalysis",
                column: "ObjectType");

            migrationBuilder.CreateIndex(
                name: "IX_LikeAnalysis_UpdatedTime",
                table: "LikeAnalysis",
                column: "UpdatedTime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Blog");

            migrationBuilder.DropTable(
                name: "LikeAnalysis");

            migrationBuilder.DropTable(
                name: "Series");

            migrationBuilder.CreateTable(
                name: "Config",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 32, nullable: true),
                    Type = table.Column<string>(maxLength: 32, nullable: true),
                    Value = table.Column<string>(maxLength: 4000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Config", x => x.Id);
                });
        }
    }
}
