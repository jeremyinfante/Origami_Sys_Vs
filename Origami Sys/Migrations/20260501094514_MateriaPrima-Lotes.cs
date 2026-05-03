using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Origami_Sys.Migrations
{
    /// <inheritdoc />
    public partial class MateriaPrimaLotes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LoteProduccion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumeroLote = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Observaciones = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoteProduccion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MateriaPrima",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnidadMedida = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StockActual = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    StockMinimo = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MateriaPrima", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LoteProducto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoteProduccionId = table.Column<int>(type: "int", nullable: false),
                    ProductoId = table.Column<int>(type: "int", nullable: false),
                    CantidadProducida = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoteProducto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoteProducto_LoteProduccion_LoteProduccionId",
                        column: x => x.LoteProduccionId,
                        principalTable: "LoteProduccion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LoteProducto_Producto_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Producto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompraMateriaPrima",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MateriaPrimaId = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Cantidad = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    CostoTotal = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Proveedor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Observaciones = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompraMateriaPrima", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompraMateriaPrima_MateriaPrima_MateriaPrimaId",
                        column: x => x.MateriaPrimaId,
                        principalTable: "MateriaPrima",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LoteDetalle",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoteProduccionId = table.Column<int>(type: "int", nullable: false),
                    MateriaPrimaId = table.Column<int>(type: "int", nullable: false),
                    CantidadUsada = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoteDetalle", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoteDetalle_LoteProduccion_LoteProduccionId",
                        column: x => x.LoteProduccionId,
                        principalTable: "LoteProduccion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LoteDetalle_MateriaPrima_MateriaPrimaId",
                        column: x => x.MateriaPrimaId,
                        principalTable: "MateriaPrima",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompraMateriaPrima_MateriaPrimaId",
                table: "CompraMateriaPrima",
                column: "MateriaPrimaId");

            migrationBuilder.CreateIndex(
                name: "IX_LoteDetalle_LoteProduccionId",
                table: "LoteDetalle",
                column: "LoteProduccionId");

            migrationBuilder.CreateIndex(
                name: "IX_LoteDetalle_MateriaPrimaId",
                table: "LoteDetalle",
                column: "MateriaPrimaId");

            migrationBuilder.CreateIndex(
                name: "IX_LoteProducto_LoteProduccionId",
                table: "LoteProducto",
                column: "LoteProduccionId");

            migrationBuilder.CreateIndex(
                name: "IX_LoteProducto_ProductoId",
                table: "LoteProducto",
                column: "ProductoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompraMateriaPrima");

            migrationBuilder.DropTable(
                name: "LoteDetalle");

            migrationBuilder.DropTable(
                name: "LoteProducto");

            migrationBuilder.DropTable(
                name: "MateriaPrima");

            migrationBuilder.DropTable(
                name: "LoteProduccion");
        }
    }
}
