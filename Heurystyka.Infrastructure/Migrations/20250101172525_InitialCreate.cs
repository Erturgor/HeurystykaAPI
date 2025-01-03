using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Heurystyka.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AlgorithmResults",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    AlgorithmName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlgorithmResults", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AlgorithmParameters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ParameterName = table.Column<string>(type: "TEXT", nullable: false),
                    ParameterValue = table.Column<double>(type: "REAL", nullable: false),
                    AlgorithmResultId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlgorithmParameters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AlgorithmParameters_AlgorithmResults_AlgorithmResultId",
                        column: x => x.AlgorithmResultId,
                        principalTable: "AlgorithmResults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlgorithmParameters_AlgorithmResultId",
                table: "AlgorithmParameters",
                column: "AlgorithmResultId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlgorithmParameters");

            migrationBuilder.DropTable(
                name: "AlgorithmResults");
        }
    }
}
