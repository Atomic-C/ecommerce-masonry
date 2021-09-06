using Microsoft.EntityFrameworkCore.Migrations;

namespace ecommerce_masonry.Migrations
{
    public partial class AddApplicationTypeIdToProduct_Rename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_ApplicationType_ApplicationId",
                table: "Product");

            migrationBuilder.RenameColumn(
                name: "ApplicationId",
                table: "Product",
                newName: "ApplicationTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Product_ApplicationId",
                table: "Product",
                newName: "IX_Product_ApplicationTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_ApplicationType_ApplicationTypeId",
                table: "Product",
                column: "ApplicationTypeId",
                principalTable: "ApplicationType",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_ApplicationType_ApplicationTypeId",
                table: "Product");

            migrationBuilder.RenameColumn(
                name: "ApplicationTypeId",
                table: "Product",
                newName: "ApplicationId");

            migrationBuilder.RenameIndex(
                name: "IX_Product_ApplicationTypeId",
                table: "Product",
                newName: "IX_Product_ApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_ApplicationType_ApplicationId",
                table: "Product",
                column: "ApplicationId",
                principalTable: "ApplicationType",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
