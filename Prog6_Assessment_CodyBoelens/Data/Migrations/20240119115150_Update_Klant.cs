using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prog6_Assessment_CodyBoelens.Data.Migrations
{
    public partial class Update_Klant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Klant",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Klant");
        }
    }
}
