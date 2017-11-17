using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WebAdmin.Migrations
{
    public partial class eventVideo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "C9Event",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    EventDate = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    EventName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Language = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    SourceUrl = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    ThumbnailUrl = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    TopicName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_C9Event", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EventVideo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    C9EventId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Caption = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Duration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mp3Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mp4HigUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mp4LowUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mp4MidUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SeriesTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SeriesTitleUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SeriesType = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    SourceUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    Tags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThumbnailUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VideoEmbed = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Views = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventVideo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventVideo_C9Event_C9EventId",
                        column: x => x.C9EventId,
                        principalTable: "C9Event",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventVideo_C9EventId",
                table: "EventVideo",
                column: "C9EventId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventVideo");

            migrationBuilder.DropTable(
                name: "C9Event");
        }
    }
}
