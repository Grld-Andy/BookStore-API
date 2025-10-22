using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookStore.Migrations
{
    /// <inheritdoc />
    public partial class AddBooks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "author", "title", "yearPublished" },
                values: new object[,]
                {
                    { 1, "Andrew Hunt and David Thomas", "The Pragmatic Programmer", 1999 },
                    { 2, "Robert C. Martin", "Clean Code", 2008 },
                    { 3, "Erich Gamma, Richard Helm, Ralph Johnson, John Vlissides", "Design Patterns: Elements of Reusable Object-Oriented Software", 1994 },
                    { 4, "Martin Fowler", "Refactoring: Improving the Design of Existing Code", 1999 },
                    { 5, "Eric Evans", "Domain-Driven Design: Tackling Complexity in the Heart of Software", 2003 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
