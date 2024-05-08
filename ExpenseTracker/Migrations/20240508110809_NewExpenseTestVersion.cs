using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseTracker.Migrations
{
    /// <inheritdoc />
    public partial class NewExpenseTestVersion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE Expenses SET Category = 0 WHERE Category = 'Mat'");
            migrationBuilder.Sql("UPDATE Expenses SET Category = 1 WHERE Category = 'Kläder'");
            migrationBuilder.Sql("UPDATE Expenses SET Category = 2 WHERE Category = 'Övrigt'");
            // Byt sedan kolumnens datatyp
            migrationBuilder.AlterColumn<int>(
                name: "Category",
                table: "Expenses",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Konvertera tillbaka till strängar vid rollback
            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "Expenses",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int));
            migrationBuilder.Sql("UPDATE Expenses SET Category = 'Mat' WHERE Category = 0");
            migrationBuilder.Sql("UPDATE Expenses SET Category = 'Kläder' WHERE Category = 1");
            migrationBuilder.Sql("UPDATE Expenses SET Category = 'Övrigt' WHERE Category = 2");
        }
    }
}
