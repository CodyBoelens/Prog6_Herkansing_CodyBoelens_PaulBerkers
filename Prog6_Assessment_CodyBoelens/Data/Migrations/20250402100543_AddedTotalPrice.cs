using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace Prog6_Assessment_CodyBoelens.Data.Migrations
{
    [ExcludeFromCodeCoverage]
    public partial class AddedTotalPrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "TotaalPrijs",
                table: "Boeking",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateIndex(
                name: "IX_Beestje_TypeId",
                table: "Beestje",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Beestje_Types_TypeId",
                table: "Beestje",
                column: "TypeId",
                principalTable: "Types",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Beestje_Types_TypeId",
                table: "Beestje");

            migrationBuilder.DropIndex(
                name: "IX_Beestje_TypeId",
                table: "Beestje");

            migrationBuilder.DropColumn(
                name: "TotaalPrijs",
                table: "Boeking");
        }
    }
}
