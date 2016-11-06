using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace voh_asp.Data.Migrations
{
    public partial class AddAppFiles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppFiles",
                columns: table => new
                {
                    AppFileId = table.Column<Guid>(nullable: false),
                    AWSBucket = table.Column<string>(nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Path = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppFiles", x => x.AppFileId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppFiles");
        }
    }
}
