using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace airmodel_ad.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "category",
                columns: table => new
                {
                    categoryId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    categoryName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    delete = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_category", x => x.categoryId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    userId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    userName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    userEmail = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    userPassword = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    userRole = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.userId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    productId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    categoryId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    productName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    productDescription = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    productImage = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    productBasicPrice = table.Column<int>(type: "int", nullable: false),
                    productQty = table.Column<int>(type: "int", nullable: false),
                    hasVarients = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_products", x => x.productId);
                    table.ForeignKey(
                        name: "FK_products_category_categoryId",
                        column: x => x.categoryId,
                        principalTable: "category",
                        principalColumn: "categoryId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "carts",
                columns: table => new
                {
                    cartId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    userId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_carts", x => x.cartId);
                    table.ForeignKey(
                        name: "FK_carts_users_userId",
                        column: x => x.userId,
                        principalTable: "users",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "orders",
                columns: table => new
                {
                    oId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    additionalCost = table.Column<int>(type: "int", nullable: false),
                    userId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    total = table.Column<int>(type: "int", nullable: false),
                    fName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    lName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    county = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    address = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    city = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    postCode = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    phoneNumber = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    orderStatus = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    orderNote = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    orderAdminNote = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    orderTime = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orders", x => x.oId);
                    table.ForeignKey(
                        name: "FK_orders_users_userId",
                        column: x => x.userId,
                        principalTable: "users",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "varient",
                columns: table => new
                {
                    varientId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    productId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    productName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_varient", x => x.varientId);
                    table.ForeignKey(
                        name: "FK_varient_products_productId",
                        column: x => x.productId,
                        principalTable: "products",
                        principalColumn: "productId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "varientOption",
                columns: table => new
                {
                    varientOptionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    varientId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    varientPrice = table.Column<int>(type: "int", nullable: false),
                    varientName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    varientImage = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    varientQty = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_varientOption", x => x.varientOptionId);
                    table.ForeignKey(
                        name: "FK_varientOption_varient_varientId",
                        column: x => x.varientId,
                        principalTable: "varient",
                        principalColumn: "varientId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "cartItems",
                columns: table => new
                {
                    cartItemId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    cartId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    productId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    qty = table.Column<int>(type: "int", nullable: false),
                    total = table.Column<int>(type: "int", nullable: false),
                    additionalCost = table.Column<int>(type: "int", nullable: false),
                    varientOptionId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cartItems", x => x.cartItemId);
                    table.ForeignKey(
                        name: "FK_cartItems_carts_cartId",
                        column: x => x.cartId,
                        principalTable: "carts",
                        principalColumn: "cartId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_cartItems_products_productId",
                        column: x => x.productId,
                        principalTable: "products",
                        principalColumn: "productId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_cartItems_varientOption_varientOptionId",
                        column: x => x.varientOptionId,
                        principalTable: "varientOption",
                        principalColumn: "varientOptionId");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "orderItem",
                columns: table => new
                {
                    oItemId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    oId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    productId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    qty = table.Column<int>(type: "int", nullable: false),
                    varientOptionId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orderItem", x => x.oItemId);
                    table.ForeignKey(
                        name: "FK_orderItem_orders_oId",
                        column: x => x.oId,
                        principalTable: "orders",
                        principalColumn: "oId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_orderItem_products_productId",
                        column: x => x.productId,
                        principalTable: "products",
                        principalColumn: "productId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_orderItem_varientOption_varientOptionId",
                        column: x => x.varientOptionId,
                        principalTable: "varientOption",
                        principalColumn: "varientOptionId");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_cartItems_cartId",
                table: "cartItems",
                column: "cartId");

            migrationBuilder.CreateIndex(
                name: "IX_cartItems_productId",
                table: "cartItems",
                column: "productId");

            migrationBuilder.CreateIndex(
                name: "IX_cartItems_varientOptionId",
                table: "cartItems",
                column: "varientOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_carts_userId",
                table: "carts",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_orderItem_oId",
                table: "orderItem",
                column: "oId");

            migrationBuilder.CreateIndex(
                name: "IX_orderItem_productId",
                table: "orderItem",
                column: "productId");

            migrationBuilder.CreateIndex(
                name: "IX_orderItem_varientOptionId",
                table: "orderItem",
                column: "varientOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_orders_userId",
                table: "orders",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_products_categoryId",
                table: "products",
                column: "categoryId");

            migrationBuilder.CreateIndex(
                name: "IX_varient_productId",
                table: "varient",
                column: "productId");

            migrationBuilder.CreateIndex(
                name: "IX_varientOption_varientId",
                table: "varientOption",
                column: "varientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cartItems");

            migrationBuilder.DropTable(
                name: "orderItem");

            migrationBuilder.DropTable(
                name: "carts");

            migrationBuilder.DropTable(
                name: "orders");

            migrationBuilder.DropTable(
                name: "varientOption");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "varient");

            migrationBuilder.DropTable(
                name: "products");

            migrationBuilder.DropTable(
                name: "category");
        }
    }
}
