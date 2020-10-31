using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineStore.Dal.Migrations
{
    public partial class FileUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Files",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "Files",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<byte[]>(
                name: "thumbnail",
                table: "Files",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Banned",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Vat",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "thumbnail",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "Banned",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Vat",
                table: "AspNetUsers");
        }
    }
}
