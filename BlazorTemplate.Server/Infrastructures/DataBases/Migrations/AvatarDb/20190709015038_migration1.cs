using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BlazorTemplate.Server.Infrastructures.DataBases.Migrations.AvatarDb
{
    public partial class migration1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Avatars_User_OwnerId",
                table: "Avatars");

            migrationBuilder.DropIndex(
                name: "IX_Avatars_OwnerId",
                table: "Avatars");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Avatars");

            migrationBuilder.AddColumn<Guid>(
                name: "User",
                table: "Avatars",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Avatars_User",
                table: "Avatars",
                column: "User");

            migrationBuilder.AddForeignKey(
                name: "FK_Avatars_User_User",
                table: "Avatars",
                column: "User",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Avatars_User_User",
                table: "Avatars");

            migrationBuilder.DropIndex(
                name: "IX_Avatars_User",
                table: "Avatars");

            migrationBuilder.DropColumn(
                name: "User",
                table: "Avatars");

            migrationBuilder.AddColumn<Guid>(
                name: "OwnerId",
                table: "Avatars",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Avatars_OwnerId",
                table: "Avatars",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Avatars_User_OwnerId",
                table: "Avatars",
                column: "OwnerId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
