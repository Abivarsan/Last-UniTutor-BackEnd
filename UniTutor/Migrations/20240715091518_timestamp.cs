using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniTutor.Migrations
{
    /// <inheritdoc />
    public partial class timestamp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TransactionTime",
                table: "Transactions",
                newName: "timestamp");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Comments",
                newName: "timestamp");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "timestamp",
                table: "Transactions",
                newName: "TransactionTime");

            migrationBuilder.RenameColumn(
                name: "timestamp",
                table: "Comments",
                newName: "Date");
        }
    }
}
