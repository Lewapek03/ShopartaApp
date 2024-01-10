using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shoparta.Data.Migrations
{
    /// <inheritdoc />
    public partial class Renameing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShippingCartId",
                table: "CardDetail");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ShippingCartId",
                table: "CardDetail",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
