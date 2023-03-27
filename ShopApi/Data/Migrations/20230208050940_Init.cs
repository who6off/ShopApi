using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopApi.Data.Migrations
{
	public partial class Init : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "Categories",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
					DisplayOrder = table.Column<int>(type: "int", nullable: false),
					IsForAdults = table.Column<bool>(type: "bit", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Categories", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "Roles",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Roles", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "Users",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Email = table.Column<string>(type: "nvarchar(320)", maxLength: 320, nullable: false),
					PasswordHash = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
					FirstName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
					SecondName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
					RoleId = table.Column<int>(type: "int", nullable: false),
					BirthDate = table.Column<DateTime>(type: "date", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Users", x => x.Id);
					table.ForeignKey(
						name: "FK_Users_Roles_RoleId",
						column: x => x.RoleId,
						principalTable: "Roles",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateTable(
				name: "Orders",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Date = table.Column<DateTime>(type: "date", nullable: false),
					IsRequestedForDelivery = table.Column<bool>(type: "bit", nullable: false),
					BuyerId = table.Column<int>(type: "int", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Orders", x => x.Id);
					table.ForeignKey(
						name: "FK_Orders_Users_BuyerId",
						column: x => x.BuyerId,
						principalTable: "Users",
						principalColumn: "Id",
						onDelete: ReferentialAction.SetNull);
				});

			migrationBuilder.CreateTable(
				name: "Products",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
					Price = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
					CategoryId = table.Column<int>(type: "int", nullable: true),
					SellerId = table.Column<int>(type: "int", nullable: false),
					Image = table.Column<string>(type: "nvarchar(max)", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Products", x => x.Id);
					table.ForeignKey(
						name: "FK_Products_Categories_CategoryId",
						column: x => x.CategoryId,
						principalTable: "Categories",
						principalColumn: "Id",
						onDelete: ReferentialAction.SetNull);
					table.ForeignKey(
						name: "FK_Products_Users_SellerId",
						column: x => x.SellerId,
						principalTable: "Users",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "OrderItem",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					OrderId = table.Column<int>(type: "int", nullable: true),
					ProductId = table.Column<int>(type: "int", nullable: true),
					Amount = table.Column<long>(type: "bigint", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_OrderItem", x => x.Id);
					table.ForeignKey(
						name: "FK_OrderItem_Orders_OrderId",
						column: x => x.OrderId,
						principalTable: "Orders",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_OrderItem_Products_ProductId",
						column: x => x.ProductId,
						principalTable: "Products",
						principalColumn: "Id");
				});

			migrationBuilder.InsertData(
				table: "Roles",
				columns: new[] { "Id", "Name" },
				values: new object[] { 1, "Admin" });

			migrationBuilder.InsertData(
				table: "Roles",
				columns: new[] { "Id", "Name" },
				values: new object[] { 2, "Seller" });

			migrationBuilder.InsertData(
				table: "Roles",
				columns: new[] { "Id", "Name" },
				values: new object[] { 3, "Buyer" });

			migrationBuilder.InsertData(
				table: "Users",
				columns: new[] { "Id", "BirthDate", "Email", "FirstName", "PasswordHash", "RoleId", "SecondName" },
				values: new object[] { 1, new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "root@hello.ca", "Ivan", "$2a$11$n9SN71tZkh9a4siKTqYki.FEu0S482LvvFK2K/ubcG3FOpZey2hBO", 1, "Huzikov" });

			migrationBuilder.CreateIndex(
				name: "IX_Categories_Name",
				table: "Categories",
				column: "Name",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_OrderItem_OrderId",
				table: "OrderItem",
				column: "OrderId");

			migrationBuilder.CreateIndex(
				name: "IX_OrderItem_ProductId",
				table: "OrderItem",
				column: "ProductId");

			migrationBuilder.CreateIndex(
				name: "IX_Orders_BuyerId",
				table: "Orders",
				column: "BuyerId");

			migrationBuilder.CreateIndex(
				name: "IX_Products_CategoryId",
				table: "Products",
				column: "CategoryId");

			migrationBuilder.CreateIndex(
				name: "IX_Products_SellerId",
				table: "Products",
				column: "SellerId");

			migrationBuilder.CreateIndex(
				name: "IX_Roles_Name",
				table: "Roles",
				column: "Name",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_Users_Email",
				table: "Users",
				column: "Email",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_Users_RoleId",
				table: "Users",
				column: "RoleId");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "OrderItem");

			migrationBuilder.DropTable(
				name: "Orders");

			migrationBuilder.DropTable(
				name: "Products");

			migrationBuilder.DropTable(
				name: "Categories");

			migrationBuilder.DropTable(
				name: "Users");

			migrationBuilder.DropTable(
				name: "Roles");
		}
	}
}
