using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prog6_Assessment_CodyBoelens.Data.Migrations
{
    public partial class AddKlantIdToBoeking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "KlantId",
                table: "Boeking",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Boeking_KlantId",
                table: "Boeking",
                column: "KlantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Boeking_Klant_KlantId",
                table: "Boeking",
                column: "KlantId",
                principalTable: "Klant",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Boeking_Klant_KlantId",
                table: "Boeking");

            migrationBuilder.DropIndex(
                name: "IX_Boeking_KlantId",
                table: "Boeking");

            migrationBuilder.DropColumn(
                name: "KlantId",
                table: "Boeking");
        }
    }
}