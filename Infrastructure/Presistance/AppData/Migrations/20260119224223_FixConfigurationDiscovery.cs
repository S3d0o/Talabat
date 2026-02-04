using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistance.AppData.Migrations
{
    /// <inheritdoc />
    public partial class FixConfigurationDiscovery : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Orders_Orderid",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_DeliveryMethods_DeliveryMethodId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductBrands_ProductBrandid",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductTypes_ProductTypeid",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_ProductBrandid",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_ProductTypeid",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductBrandid",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductTypeid",
                table: "Products");

            migrationBuilder.AlterColumn<decimal>(
                name: "SubTotal",
                table: "Orders",
                type: "decimal(18,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<string>(
                name: "PaymentStatus",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "OrderItems",
                type: "decimal(18,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "DeliveryMethods",
                type: "decimal(18,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.CreateIndex(
                name: "IX_Products_BrandId",
                table: "Products",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_TypeId",
                table: "Products",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Orders_Orderid",
                table: "OrderItems",
                column: "Orderid",
                principalTable: "Orders",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_DeliveryMethods_DeliveryMethodId",
                table: "Orders",
                column: "DeliveryMethodId",
                principalTable: "DeliveryMethods",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductBrands_BrandId",
                table: "Products",
                column: "BrandId",
                principalTable: "ProductBrands",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductTypes_TypeId",
                table: "Products",
                column: "TypeId",
                principalTable: "ProductTypes",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Orders_Orderid",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_DeliveryMethods_DeliveryMethodId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductBrands_BrandId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductTypes_TypeId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_BrandId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_TypeId",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "ProductBrandid",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductTypeid",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<decimal>(
                name: "SubTotal",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)");

            migrationBuilder.AlterColumn<int>(
                name: "PaymentStatus",
                table: "Orders",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "OrderItems",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "DeliveryMethods",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductBrandid",
                table: "Products",
                column: "ProductBrandid");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductTypeid",
                table: "Products",
                column: "ProductTypeid");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Orders_Orderid",
                table: "OrderItems",
                column: "Orderid",
                principalTable: "Orders",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_DeliveryMethods_DeliveryMethodId",
                table: "Orders",
                column: "DeliveryMethodId",
                principalTable: "DeliveryMethods",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductBrands_ProductBrandid",
                table: "Products",
                column: "ProductBrandid",
                principalTable: "ProductBrands",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductTypes_ProductTypeid",
                table: "Products",
                column: "ProductTypeid",
                principalTable: "ProductTypes",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
