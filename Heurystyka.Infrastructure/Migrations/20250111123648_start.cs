using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Heurystyka.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class start : Migration
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
                name: "ReportMultiples",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportMultiples", x => x.Id);
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

            migrationBuilder.CreateTable(
                name: "ReportSingle",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    XBest = table.Column<string>(type: "TEXT", nullable: false),
                    FBest = table.Column<double>(type: "REAL", nullable: false),
                    AlgorithmName = table.Column<string>(type: "TEXT", nullable: false),
                    AlgorithmFunction = table.Column<string>(type: "TEXT", nullable: false),
                    Parameters = table.Column<string>(type: "TEXT", nullable: true),
                    ReportMultipleId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportSingle", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportSingle_ReportMultiples_ReportMultipleId",
                        column: x => x.ReportMultipleId,
                        principalTable: "ReportMultiples",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlgorithmParameters_AlgorithmResultId",
                table: "AlgorithmParameters",
                column: "AlgorithmResultId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportSingle_ReportMultipleId",
                table: "ReportSingle",
                column: "ReportMultipleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlgorithmParameters");

            migrationBuilder.DropTable(
                name: "ReportSingle");

            migrationBuilder.DropTable(
                name: "AlgorithmResults");

            migrationBuilder.DropTable(
                name: "ReportMultiples");
        }
    }
}
