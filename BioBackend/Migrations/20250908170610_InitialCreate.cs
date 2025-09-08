using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BioBackend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Diseases",
                columns: table => new
                {
                    DiseaseID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DiseaseName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diseases", x => x.DiseaseID);
                });

            migrationBuilder.CreateTable(
                name: "Drugs",
                columns: table => new
                {
                    DrugID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DrugName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drugs", x => x.DrugID);
                });

            migrationBuilder.CreateTable(
                name: "Genes",
                columns: table => new
                {
                    GeneID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GeneName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genes", x => x.GeneID);
                });

            migrationBuilder.CreateTable(
                name: "DiseaseGenes",
                columns: table => new
                {
                    DiseaseID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GeneID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EvidenceType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Strength = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiseaseGenes", x => new { x.DiseaseID, x.GeneID });
                    table.ForeignKey(
                        name: "FK_DiseaseGenes_Diseases_DiseaseID",
                        column: x => x.DiseaseID,
                        principalTable: "Diseases",
                        principalColumn: "DiseaseID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DiseaseGenes_Genes_GeneID",
                        column: x => x.GeneID,
                        principalTable: "Genes",
                        principalColumn: "GeneID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DrugGenes",
                columns: table => new
                {
                    DrugID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GeneID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Effect = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApprovalYear = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrugGenes", x => new { x.DrugID, x.GeneID });
                    table.ForeignKey(
                        name: "FK_DrugGenes_Drugs_DrugID",
                        column: x => x.DrugID,
                        principalTable: "Drugs",
                        principalColumn: "DrugID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DrugGenes_Genes_GeneID",
                        column: x => x.GeneID,
                        principalTable: "Genes",
                        principalColumn: "GeneID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiseaseGenes_GeneID",
                table: "DiseaseGenes",
                column: "GeneID");

            migrationBuilder.CreateIndex(
                name: "IX_DrugGenes_GeneID",
                table: "DrugGenes",
                column: "GeneID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiseaseGenes");

            migrationBuilder.DropTable(
                name: "DrugGenes");

            migrationBuilder.DropTable(
                name: "Diseases");

            migrationBuilder.DropTable(
                name: "Drugs");

            migrationBuilder.DropTable(
                name: "Genes");
        }
    }
}
