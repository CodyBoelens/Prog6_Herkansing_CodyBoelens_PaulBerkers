using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prog6_Assessment_CodyBoelens.Data.Migrations
{
    public partial class Types : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Beestje");

            migrationBuilder.AddColumn<int>(
                name: "TypeId",
                table: "Beestje",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Types",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Types", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Types");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "Beestje");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Beestje",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
