using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SIGRHBack.Migrations
{
    public partial class update_name_table_tokenInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TokenInfo",
                table: "TokenInfo");

            migrationBuilder.RenameTable(
                name: "TokenInfo",
                newName: "TD_TokenInfo");

            migrationBuilder.AlterColumn<string>(
                name: "PrenomUtilisateur",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TD_TokenInfo",
                table: "TD_TokenInfo",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TD_TokenInfo",
                table: "TD_TokenInfo");

            migrationBuilder.RenameTable(
                name: "TD_TokenInfo",
                newName: "TokenInfo");

            migrationBuilder.AlterColumn<string>(
                name: "PrenomUtilisateur",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TokenInfo",
                table: "TokenInfo",
                column: "Id");
        }
    }
}
