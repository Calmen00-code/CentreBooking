using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CentreBookingDatabase.Migrations
{
    /// <inheritdoc />
    public partial class fkmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Centres",
                columns: table => new
                {
                    CentreName = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Centres", x => x.CentreName);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    CentreName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StartDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EndDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GuestName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CentreName1 = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.CentreName);
                    table.ForeignKey(
                        name: "FK_Bookings_Centres_CentreName1",
                        column: x => x.CentreName1,
                        principalTable: "Centres",
                        principalColumn: "CentreName");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_CentreName1",
                table: "Bookings",
                column: "CentreName1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "Centres");
        }
    }
}
