using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddDrugGeneRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Genes_Drugs_DrugId",
                table: "Genes");

            migrationBuilder.DropIndex(
                name: "IX_Genes_DrugId",
                table: "Genes");

            migrationBuilder.DropColumn(
                name: "DrugId",
                table: "Genes");

            migrationBuilder.CreateTable(
                name: "DrugGene",
                columns: table => new
                {
                    TargetGenesId = table.Column<int>(type: "int", nullable: false),
                    TargetedByDrugsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrugGene", x => new { x.TargetGenesId, x.TargetedByDrugsId });
                    table.ForeignKey(
                        name: "FK_DrugGene_Drugs_TargetedByDrugsId",
                        column: x => x.TargetedByDrugsId,
                        principalTable: "Drugs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DrugGene_Genes_TargetGenesId",
                        column: x => x.TargetGenesId,
                        principalTable: "Genes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DrugGene_TargetedByDrugsId",
                table: "DrugGene",
                column: "TargetedByDrugsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DrugGene");

            migrationBuilder.AddColumn<int>(
                name: "DrugId",
                table: "Genes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Genes_DrugId",
                table: "Genes",
                column: "DrugId");

            migrationBuilder.AddForeignKey(
                name: "FK_Genes_Drugs_DrugId",
                table: "Genes",
                column: "DrugId",
                principalTable: "Drugs",
                principalColumn: "Id");
        }
    }
}
