using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prog6_Assessment_CodyBoelens.Data.Migrations
{
    public partial class Boekingen : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Boeking",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Adress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Is_Confirmed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boeking", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BeestjeBoeking",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BeestjeID = table.Column<int>(type: "int", nullable: false),
                    BoekingID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BeestjeBoeking", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BeestjeBoeking_Beestje_BeestjeID",
                        column: x => x.BeestjeID,
                        principalTable: "Beestje",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BeestjeBoeking_Boeking_BoekingID",
                        column: x => x.BoekingID,
                        principalTable: "Boeking",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BeestjeBoeking_BeestjeID",
                table: "BeestjeBoeking",
                column: "BeestjeID");

            migrationBuilder.CreateIndex(
                name: "IX_BeestjeBoeking_BoekingID",
                table: "BeestjeBoeking",
                column: "BoekingID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BeestjeBoeking");

            migrationBuilder.DropTable(
                name: "Boeking");
        }
    }
}
