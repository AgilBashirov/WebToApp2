using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebToApp2.Migrations
{
    /// <inheritdoc />
    public partial class OperationFileIsSignedAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSigned",
                table: "OperationFiles",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSigned",
                table: "OperationFiles");
        }
    }
}
