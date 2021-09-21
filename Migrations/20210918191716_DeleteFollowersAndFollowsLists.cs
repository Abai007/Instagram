using Microsoft.EntityFrameworkCore.Migrations;

namespace homework_59.Migrations
{
    public partial class DeleteFollowersAndFollowsLists : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserObjUserObj");

            migrationBuilder.AddColumn<int>(
                name: "FollowCount",
                table: "AspNetUsers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FollowerCount",
                table: "AspNetUsers",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FollowCount",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FollowerCount",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "UserObjUserObj",
                columns: table => new
                {
                    FollowId = table.Column<string>(type: "text", nullable: false),
                    FollowerId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserObjUserObj", x => new { x.FollowId, x.FollowerId });
                    table.ForeignKey(
                        name: "FK_UserObjUserObj_AspNetUsers_FollowerId",
                        column: x => x.FollowerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserObjUserObj_AspNetUsers_FollowId",
                        column: x => x.FollowId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserObjUserObj_FollowerId",
                table: "UserObjUserObj",
                column: "FollowerId");
        }
    }
}
