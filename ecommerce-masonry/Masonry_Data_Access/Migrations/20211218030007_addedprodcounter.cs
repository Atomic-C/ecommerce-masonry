using Microsoft.EntityFrameworkCore.Migrations;

namespace Masonry_Data_Access.Migrations
{
    public partial class addedprodcounter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProdCounter",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProdCounter",
                table: "Product");
        }
    }
}
