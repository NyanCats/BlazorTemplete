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
                    Id = table.Column<Guid>(nullable: false),
                    Image = table.Column<byte[]>(nullable: true),
                    LastUpdated = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Avatars", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserAvatar",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    AvatarId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAvatar", x => x.UserId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Avatars");

            migrationBuilder.DropTable(
                name: "UserAvatar");
        }
    }
}
