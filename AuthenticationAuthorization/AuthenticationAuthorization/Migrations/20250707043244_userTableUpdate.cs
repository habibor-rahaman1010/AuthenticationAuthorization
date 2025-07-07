using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthenticationAuthorization.Migrations
{
    /// <inheritdoc />
    public partial class userTableUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FistName",
                table: "Users",
                newName: "FirstName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "Users",
                newName: "FistName");
        }
    }
}
