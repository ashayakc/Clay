using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Offices",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Doors",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OfficeId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Doors_OfficeId",
                        column: x => x.OfficeId,
                        principalTable: "Offices",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Firstname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmployeeId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsAdmin = table.Column<byte>(type: "tinyint", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoleId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleDoorMappings",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    DoorId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleDoorMappings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleDoorMapping_DoorId",
                        column: x => x.DoorId,
                        principalTable: "Doors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RoleDoorMapping_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Offices",
                columns: new[] { "Id", "Address", "Name" },
                values: new object[,]
                {
                    { 1L, "Amsterdam", "Clay" },
                    { 2L, "Eindhoven", "Philips" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1L, "", "Manager" },
                    { 2L, "", "Director" },
                    { 3L, "", "Developer" },
                    { 4L, "", "Finance" }
                });

            migrationBuilder.InsertData(
                table: "Doors",
                columns: new[] { "Id", "Name", "OfficeId", "Type" },
                values: new object[,]
                {
                    { 1L, "FrontDoor", 1L, "Main" },
                    { 2L, "BackDoor", 1L, "Back" },
                    { 3L, "StoreRoom", 1L, "Store" },
                    { 4L, "FrontDoor", 2L, "Main" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "EmailId", "EmployeeId", "Firstname", "IsAdmin", "LastName", "Password", "RoleId", "UserName" },
                values: new object[,]
                {
                    { 1L, "sheldoncooper@gmail.com", "1000", "Sheldon", (byte)1, "Cooper", "c2hlbGRvbg==", 1L, "sheldon" },
                    { 2L, "adia@gmail.com", "1001", "Adia", (byte)0, "Bugg", "YWRpYQ==", 2L, "adia" },
                    { 3L, "olive@gmail.com", "1002", "Olive", (byte)0, "yew", "b2xpdmU=", 3L, "olive" },
                    { 4L, "peg@gmail.com", "1003", "Peg", (byte)0, "Legge", "cGVn", 3L, "peg" },
                    { 5L, "allie@gmail.com", "1004", "Allie", (byte)0, "Grater", "YWxsaWU=", 4L, "allie" }
                });

            migrationBuilder.InsertData(
                table: "RoleDoorMappings",
                columns: new[] { "Id", "DoorId", "RoleId" },
                values: new object[,]
                {
                    { 1L, 1L, 1L },
                    { 2L, 1L, 2L },
                    { 3L, 1L, 3L },
                    { 4L, 1L, 4L },
                    { 5L, 2L, 1L },
                    { 6L, 2L, 2L },
                    { 7L, 3L, 3L },
                    { 8L, 4L, 4L }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Doors_OfficeId",
                table: "Doors",
                column: "OfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleDoorMapping_RoleId_DoorId",
                table: "RoleDoorMappings",
                columns: new[] { "RoleId", "DoorId" });

            migrationBuilder.CreateIndex(
                name: "IX_RoleDoorMappings_DoorId",
                table: "RoleDoorMappings",
                column: "DoorId");

            migrationBuilder.CreateIndex(
                name: "IX_User_UserName",
                table: "Users",
                column: "UserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoleDoorMappings");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Doors");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Offices");
        }
    }
}
