using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CentreBookingDatabase.Migrations
{
    /// <inheritdoc />
    public partial class AddingCentreAndRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Centres_CentreName1",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_CentreName1",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "CentreName1",
                table: "Bookings");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Centres_CentreName",
                table: "Bookings",
                column: "CentreName",
                principalTable: "Centres",
                principalColumn: "CentreName",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Centres_CentreName",
                table: "Bookings");

            migrationBuilder.AddColumn<string>(
                name: "CentreName1",
                table: "Bookings",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_CentreName1",
                table: "Bookings",
                column: "CentreName1");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Centres_CentreName1",
                table: "Bookings",
                column: "CentreName1",
                principalTable: "Centres",
                principalColumn: "CentreName");
        }
    }
}
