using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WebAdmin.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BingNews",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Provider = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: true),
                    Tags = table.Column<string>(nullable: true),
                    ThumbnailUrl = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    UpdatedTime = table.Column<DateTime>(nullable: false),
                    Url = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BingNews", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "C9Articles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    Duration = table.Column<string>(maxLength: 32, nullable: true),
                    SeriesTitle = table.Column<string>(maxLength: 128, nullable: true),
                    SeriesTitleUrl = table.Column<string>(maxLength: 256, nullable: true),
                    SourceUrl = table.Column<string>(maxLength: 256, nullable: true),
                    Status = table.Column<int>(nullable: true),
                    ThumbnailUrl = table.Column<string>(maxLength: 256, nullable: true),
                    Title = table.Column<string>(maxLength: 256, nullable: true),
                    UpdatedTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_C9Articles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "C9Videos",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Author = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(type: "ntext", nullable: true),
                    Duration = table.Column<string>(maxLength: 32, nullable: true),
                    Language = table.Column<string>(maxLength: 32, nullable: true),
                    Mp3Url = table.Column<string>(maxLength: 512, nullable: true),
                    Mp4HigUrl = table.Column<string>(maxLength: 512, nullable: true),
                    Mp4LowUrl = table.Column<string>(maxLength: 512, nullable: true),
                    Mp4MidUrl = table.Column<string>(maxLength: 512, nullable: true),
                    SeriesTitle = table.Column<string>(maxLength: 512, nullable: true),
                    SeriesTitleUrl = table.Column<string>(maxLength: 512, nullable: true),
                    SourceUrl = table.Column<string>(maxLength: 512, nullable: true),
                    Status = table.Column<int>(nullable: true),
                    Tags = table.Column<string>(maxLength: 512, nullable: true),
                    ThumbnailUrl = table.Column<string>(maxLength: 512, nullable: true),
                    Title = table.Column<string>(maxLength: 512, nullable: true),
                    UpdatedTime = table.Column<DateTime>(nullable: false),
                    Views = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_C9Videos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CataLog",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    IsTop = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Status = table.Column<int>(nullable: true),
                    TopCatalogId = table.Column<Guid>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    UpdatedTime = table.Column<DateTime>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CataLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CataLog_CataLog_TopCatalogId",
                        column: x => x.TopCatalogId,
                        principalTable: "CataLog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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

            migrationBuilder.CreateTable(
                name: "DevBlogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Author = table.Column<string>(maxLength: 64, nullable: true),
                    Category = table.Column<string>(maxLength: 32, nullable: true),
                    Content = table.Column<string>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    Link = table.Column<string>(maxLength: 128, nullable: true),
                    SourcContent = table.Column<string>(nullable: true),
                    SourceTitle = table.Column<string>(maxLength: 128, nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Title = table.Column<string>(maxLength: 128, nullable: true),
                    UpdatedTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DevBlogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MvaVideos",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Author = table.Column<string>(maxLength: 768, nullable: true),
                    AuthorCompany = table.Column<string>(maxLength: 384, nullable: true),
                    AuthorJobTitle = table.Column<string>(maxLength: 1024, nullable: true),
                    CourseDuration = table.Column<string>(maxLength: 32, nullable: true),
                    CourseImage = table.Column<string>(maxLength: 512, nullable: true),
                    CourseLevel = table.Column<string>(maxLength: 32, nullable: true),
                    CourseNumber = table.Column<string>(maxLength: 128, nullable: true),
                    CourseStatus = table.Column<string>(maxLength: 32, nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(maxLength: 4000, nullable: true),
                    LanguageCode = table.Column<string>(maxLength: 16, nullable: true),
                    MvaId = table.Column<int>(nullable: true),
                    ProductPackageVersionId = table.Column<int>(nullable: true),
                    SourceUrl = table.Column<string>(maxLength: 512, nullable: true),
                    Status = table.Column<int>(nullable: true),
                    Tags = table.Column<string>(maxLength: 384, nullable: true),
                    Technologies = table.Column<string>(maxLength: 384, nullable: true),
                    Title = table.Column<string>(maxLength: 256, nullable: true),
                    UpdatedTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MvaVideos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RssNews",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Author = table.Column<string>(nullable: true),
                    Categories = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    LastUpdateTime = table.Column<DateTime>(nullable: false),
                    Link = table.Column<string>(nullable: true),
                    MobileContent = table.Column<string>(nullable: true),
                    PublishId = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RssNews", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Resource",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AbsolutePath = table.Column<string>(maxLength: 256, nullable: true),
                    CatalogId = table.Column<Guid>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(maxLength: 1024, nullable: true),
                    IMGUrl = table.Column<string>(maxLength: 256, nullable: true),
                    Language = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Path = table.Column<string>(maxLength: 128, nullable: true),
                    Status = table.Column<int>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    UpdatedTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resource", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Resource_CataLog_CatalogId",
                        column: x => x.CatalogId,
                        principalTable: "CataLog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sources",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    Hash = table.Column<string>(maxLength: 256, nullable: true),
                    Name = table.Column<string>(maxLength: 128, nullable: true),
                    ResourceId = table.Column<Guid>(nullable: true),
                    Status = table.Column<int>(nullable: true),
                    Tag = table.Column<string>(maxLength: 32, nullable: true),
                    Type = table.Column<string>(maxLength: 32, nullable: true),
                    UpdatedTime = table.Column<DateTime>(nullable: false),
                    Url = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sources", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sources_Resource_ResourceId",
                        column: x => x.ResourceId,
                        principalTable: "Resource",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BingNews_Title",
                table: "BingNews",
                column: "Title",
                unique: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_CataLog_TopCatalogId",
                table: "CataLog",
                column: "TopCatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_CataLog_Value",
                table: "CataLog",
                column: "Value",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Config_Type",
                table: "Config",
                column: "Type");

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

            migrationBuilder.CreateIndex(
                name: "IX_Resource_CatalogId",
                table: "Resource",
                column: "CatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_Resource_Name",
                table: "Resource",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Sources_ResourceId",
                table: "Sources",
                column: "ResourceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BingNews");

            migrationBuilder.DropTable(
                name: "C9Articles");

            migrationBuilder.DropTable(
                name: "C9Videos");

            migrationBuilder.DropTable(
                name: "Config");

            migrationBuilder.DropTable(
                name: "DevBlogs");

            migrationBuilder.DropTable(
                name: "MvaVideos");

            migrationBuilder.DropTable(
                name: "RssNews");

            migrationBuilder.DropTable(
                name: "Sources");

            migrationBuilder.DropTable(
                name: "Resource");

            migrationBuilder.DropTable(
                name: "CataLog");
        }
    }
}
