using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BlazorTemplate.Server.Infrastructures.DataBases.Migrations.AvatarDb
{
    public partial class migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Avatars_User_OwnerId",
                table: "Avatars");

            migrationBuilder.AlterColumn<Guid>(
                name: "OwnerId",
                table: "Avatars",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Avatars_User_OwnerId",
                table: "Avatars",
                column: "OwnerId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Avatars_User_OwnerId",
                table: "Avatars");

            migrationBuilder.AlterColumn<Guid>(
                name: "OwnerId",
                table: "Avatars",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddForeignKey(
                name: "FK_Avatars_User_OwnerId",
                table: "Avatars",
                column: "OwnerId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
