using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAdmin.Migrations
{
    public partial class mvavideos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DetailDescription",
                table: "MvaVideos",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MvaDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    Duration = table.Column<DateTime>(nullable: false),
                    HighDownloadUrl = table.Column<string>(maxLength: 256, nullable: true),
                    LowDownloadUrl = table.Column<string>(maxLength: 256, nullable: true),
                    MidDownloadUrl = table.Column<string>(maxLength: 256, nullable: true),
                    MvaVideoId = table.Column<Guid>(nullable: true),
                    SourceUrl = table.Column<string>(maxLength: 256, nullable: true),
                    Status = table.Column<int>(nullable: true),
                    Title = table.Column<string>(maxLength: 128, nullable: true),
                    UpdatedTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MvaDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MvaDetails_MvaVideos_MvaVideoId",
                        column: x => x.MvaVideoId,
                        principalTable: "MvaVideos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MvaDetails_MvaVideoId",
                table: "MvaDetails",
                column: "MvaVideoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MvaDetails");

            migrationBuilder.DropColumn(
                name: "DetailDescription",
                table: "MvaVideos");
        }
    }
}
