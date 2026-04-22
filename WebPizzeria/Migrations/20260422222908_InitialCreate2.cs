using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebPizzeria.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Pizzas",
                columns: new[] { "Id", "Description", "ImageUrl", "IsVegetarian", "Name", "Price", "Size" },
                values: new object[,]
                {
                    { 1, "Томатный соус, моцарелла, базилик", "/images/margherita.jpg", true, "Маргарита", 450m, "Medium" },
                    { 2, "Пепперони, моцарелла, томатный соус", "/images/pepperoni.jpg", false, "Пепперони", 550m, "Medium" },
                    { 3, "Курица, ананас, моцарелла", "/images/hawaiian.jpg", false, "Гавайская", 600m, "Medium" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Pizzas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Pizzas",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Pizzas",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
