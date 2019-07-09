using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BlazorTemplate.Server.Infrastructures.DataBases.Migrations.AvatarDb
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Avatars",
                columns: table => new
                {
                    OwnerId = table.Column<Guid>(nullable: false),
                    Image = table.Column<byte[]>(nullable: true),
                    LastUpdated = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Avatars", x => x.OwnerId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Avatars");
        }
    }
}
