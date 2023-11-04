using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebToApp2.Migrations
{
    /// <inheritdoc />
    public partial class OperationFilesAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Operations_Files_FileId",
                table: "Operations");

            migrationBuilder.DropIndex(
                name: "IX_Operations_FileId",
                table: "Operations");

            migrationBuilder.RenameColumn(
                name: "FileId",
                table: "Operations",
                newName: "GetFileResponseType");

            migrationBuilder.CreateTable(
                name: "OperationFile",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OperationId = table.Column<int>(type: "int", nullable: false),
                    FileId = table.Column<int>(type: "int", nullable: false),
                    AppFileId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OperationFile_Files_AppFileId",
                        column: x => x.AppFileId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OperationFile_Operations_OperationId",
                        column: x => x.OperationId,
                        principalTable: "Operations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OperationFile_AppFileId",
                table: "OperationFile",
                column: "AppFileId");

            migrationBuilder.CreateIndex(
                name: "IX_OperationFile_OperationId",
                table: "OperationFile",
                column: "OperationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OperationFile");

            migrationBuilder.RenameColumn(
                name: "GetFileResponseType",
                table: "Operations",
                newName: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_Operations_FileId",
                table: "Operations",
                column: "FileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Operations_Files_FileId",
                table: "Operations",
                column: "FileId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
