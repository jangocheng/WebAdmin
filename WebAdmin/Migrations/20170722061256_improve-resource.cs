using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAdmin.Migrations
{
    public partial class improveresource : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRecommend",
                table: "Resource",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Provider",
                table: "Resource",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Tag",
                table: "Resource",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ViewNumber",
                table: "Resource",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRecommend",
                table: "Resource");

            migrationBuilder.DropColumn(
                name: "Provider",
                table: "Resource");

            migrationBuilder.DropColumn(
                name: "Tag",
                table: "Resource");

            migrationBuilder.DropColumn(
                name: "ViewNumber",
                table: "Resource");
        }
    }
}
