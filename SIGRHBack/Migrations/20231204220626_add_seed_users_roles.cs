using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SIGRHBack.Migrations
{
    public partial class add_seed_users_roles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Identifiant",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NomUtilisateur",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PrenomUtilisateur",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Identifiant",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "NomUtilisateur",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PrenomUtilisateur",
                table: "AspNetUsers");
        }
    }
}
