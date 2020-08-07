using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Source.Migrations
{
    public partial class ConvertPrimaryKeyToGuid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropPrimaryKey(
                name: "PK_Students",
                table: "Students"
            );
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Students",
                newName: "TempId"
            );
            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Students",
                nullable: true
            );
            migrationBuilder.Sql(@"
                UPDATE s
                SET s.Id = CAST(
                    CONVERT(
                        BINARY(16),
                        REVERSE(
                            CONVERT(BINARY(16), s.TempId)
                        )
                    ) as uniqueidentifier
                )
                FROM dbo.Students s
            ");
            migrationBuilder.DropColumn(
                name: "TempId",
                table: "Students"
            );
            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Students",
                nullable: false
            );
            migrationBuilder.AddPrimaryKey(
                name: "PK_Students",
                table: "Students",
                column: "Id"
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Students",
                table: "Students"
            );
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Students",
                newName: "TempId"
            );
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Students",
                nullable: true
            );
            migrationBuilder.Sql(@"
                UPDATE s
                SET s.Id = CAST(
                    CONVERT(
                        BINARY(16),
                        REVERSE(CONVERT(BINARY(16), s.TempId))
                    ) as int
                )
                FROM dbo.Students s
            ");
            migrationBuilder.DropColumn(
                name: "TempId",
                table: "Students"
            );
            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Students",
                nullable: false
            );
            migrationBuilder.AddPrimaryKey(
                name: "PK_Students",
                table: "Students",
                column: "Id"
            );

        }
    }
}
