using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopApi.Data.Migrations
{
	public partial class ChangeOrderEntity : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AlterColumn<DateTime>(
				name: "DeliveryDate",
				table: "Orders",
				type: "datetime2",
				nullable: true,
				oldClrType: typeof(DateTime),
				oldType: "datetime2");

			migrationBuilder.AddColumn<bool>(
				name: "IsCanceled",
				table: "Orders",
				type: "bit",
				nullable: false,
				defaultValue: false);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "IsCanceled",
				table: "Orders");

			migrationBuilder.AlterColumn<DateTime>(
				name: "DeliveryDate",
				table: "Orders",
				type: "datetime2",
				nullable: false,
				defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
				oldClrType: typeof(DateTime),
				oldType: "datetime2",
				oldNullable: true);
		}
	}
}
