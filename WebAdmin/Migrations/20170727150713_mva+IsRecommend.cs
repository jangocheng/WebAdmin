using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAdmin.Migrations
{
    public partial class mvaIsRecommend : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Provider",
                table: "Resource",
                maxLength: 128,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 32,
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsRecommend",
                table: "Resource",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<bool>(
                name: "IsRecommend",
                table: "MvaVideos",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRecommend",
                table: "MvaVideos");

            migrationBuilder.AlterColumn<string>(
                name: "Provider",
                table: "Resource",
                maxLength: 32,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 128,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "IsRecommend",
                table: "Resource",
                nullable: false,
                oldClrType: typeof(bool));
        }
    }
}
