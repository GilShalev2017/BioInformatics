using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiseaseID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiseaseName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diseases", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Drugs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DrugId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DrugName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drugs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Genes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GeneID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GeneName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DrugId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Genes_Drugs_DrugId",
                        column: x => x.DrugId,
                        principalTable: "Drugs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DiseaseGene",
                columns: table => new
                {
                    RelatedDiseasesId = table.Column<int>(type: "int", nullable: false),
                    RelatedGenesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiseaseGene", x => new { x.RelatedDiseasesId, x.RelatedGenesId });
                    table.ForeignKey(
                        name: "FK_DiseaseGene_Diseases_RelatedDiseasesId",
                        column: x => x.RelatedDiseasesId,
                        principalTable: "Diseases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DiseaseGene_Genes_RelatedGenesId",
                        column: x => x.RelatedGenesId,
                        principalTable: "Genes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiseaseGene_RelatedGenesId",
                table: "DiseaseGene",
                column: "RelatedGenesId");

            migrationBuilder.CreateIndex(
                name: "IX_Genes_DrugId",
                table: "Genes",
                column: "DrugId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiseaseGene");

            migrationBuilder.DropTable(
                name: "Diseases");

            migrationBuilder.DropTable(
                name: "Genes");

            migrationBuilder.DropTable(
                name: "Drugs");
        }
    }
}
