using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel.Migrations
{
    /// <inheritdoc />
    public partial class AddIdentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Bookings_Users_UserId",
                table: "User_Bookings");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_User_Bookings_UserId",
                table: "User_Bookings");

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "User_Bookings",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Phone",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfilePicturePath",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Room_bookingsId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_Bookings_UserId1",
                table: "User_Bookings",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Room_bookingsId",
                table: "AspNetUsers",
                column: "Room_bookingsId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Room_Bookings_Room_bookingsId",
                table: "AspNetUsers",
                column: "Room_bookingsId",
                principalTable: "Room_Bookings",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Bookings_AspNetUsers_UserId1",
                table: "User_Bookings",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Room_Bookings_Room_bookingsId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Bookings_AspNetUsers_UserId1",
                table: "User_Bookings");

            migrationBuilder.DropIndex(
                name: "IX_User_Bookings_UserId1",
                table: "User_Bookings");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_Room_bookingsId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "User_Bookings");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ProfilePicturePath",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Room_bookingsId",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<int>(type: "int", nullable: false),
                    ProfilePicturePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Room_bookingsId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Room_Bookings_Room_bookingsId",
                        column: x => x.Room_bookingsId,
                        principalTable: "Room_Bookings",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_Bookings_UserId",
                table: "User_Bookings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Room_bookingsId",
                table: "Users",
                column: "Room_bookingsId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Bookings_Users_UserId",
                table: "User_Bookings",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
