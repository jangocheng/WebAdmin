using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAdmin.Migrations
{
    public partial class mvadetailsequence : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Sequence",
                table: "MvaDetails",
                maxLength: 3,
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sequence",
                table: "MvaDetails");
        }
    }
}
