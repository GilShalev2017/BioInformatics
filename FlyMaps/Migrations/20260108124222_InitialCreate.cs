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
                    Symbol = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Aliases = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DbLink",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SourceDbId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SourceDb = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GeneId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbLink", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DbLink_Genes_GeneId",
                        column: x => x.GeneId,
                        principalTable: "Genes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GeneSummary",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Summary = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GeneId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneSummary", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeneSummary_Genes_GeneId",
                        column: x => x.GeneId,
                        principalTable: "Genes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DbLink_GeneId",
                table: "DbLink",
                column: "GeneId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneSummary_GeneId",
                table: "GeneSummary",
                column: "GeneId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DbLink");

            migrationBuilder.DropTable(
                name: "GeneSummary");

            migrationBuilder.DropTable(
                name: "Genes");
        }
    }
}
