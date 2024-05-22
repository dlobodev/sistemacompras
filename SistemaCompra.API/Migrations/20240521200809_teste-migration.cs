using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SistemaCompra.API.Migrations
{
    public partial class testemigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Item_SolicitacaoCompra_SolicitacaoCompraId",
                table: "Item");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Item",
                table: "Item");

            migrationBuilder.DropIndex(
                name: "IX_Item_SolicitacaoCompraId",
                table: "Item");

            migrationBuilder.AlterColumn<Guid>(
                name: "SolicitacaoCompraId",
                table: "Item",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Item",
                table: "Item",
                columns: new[] { "SolicitacaoCompraId", "Id" });

            migrationBuilder.AddForeignKey(
                name: "FK_Item_SolicitacaoCompra_SolicitacaoCompraId",
                table: "Item",
                column: "SolicitacaoCompraId",
                principalTable: "SolicitacaoCompra",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Item_SolicitacaoCompra_SolicitacaoCompraId",
                table: "Item");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Item",
                table: "Item");

            migrationBuilder.AlterColumn<Guid>(
                name: "SolicitacaoCompraId",
                table: "Item",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Item",
                table: "Item",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Item_SolicitacaoCompraId",
                table: "Item",
                column: "SolicitacaoCompraId");

            migrationBuilder.AddForeignKey(
                name: "FK_Item_SolicitacaoCompra_SolicitacaoCompraId",
                table: "Item",
                column: "SolicitacaoCompraId",
                principalTable: "SolicitacaoCompra",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
