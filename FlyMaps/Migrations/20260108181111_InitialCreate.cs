using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlyMaps.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Genes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Symbol = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DbLinks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SourceDb = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SourceDbId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GeneId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbLinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DbLinks_Genes_GeneId",
                        column: x => x.GeneId,
                        principalTable: "Genes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GeneAliases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Alias = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GeneId = table.Column<int>(type: "int", nullable: false),
                    Source = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneAliases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeneAliases_Genes_GeneId",
                        column: x => x.GeneId,
                        principalTable: "Genes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GeneSummaries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Summary = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GeneId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneSummaries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeneSummaries_Genes_GeneId",
                        column: x => x.GeneId,
                        principalTable: "Genes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DbLinks_GeneId_SourceDb",
                table: "DbLinks",
                columns: new[] { "GeneId", "SourceDb" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GeneAliases_GeneId_Alias_Source",
                table: "GeneAliases",
                columns: new[] { "GeneId", "Alias", "Source" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Genes_Symbol",
                table: "Genes",
                column: "Symbol",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GeneSummaries_GeneId",
                table: "GeneSummaries",
                column: "GeneId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DbLinks");

            migrationBuilder.DropTable(
                name: "GeneAliases");

            migrationBuilder.DropTable(
                name: "GeneSummaries");

            migrationBuilder.DropTable(
                name: "Genes");
        }
    }
}
