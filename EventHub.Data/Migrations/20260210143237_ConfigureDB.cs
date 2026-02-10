using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventHub.Data.Migrations
{
    /// <inheritdoc />
    public partial class ConfigureDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_AspNetUsers_BuyerId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Events_EventId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryEvent_Categories_CategoriesId",
                table: "CategoryEvent");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryEvent_Events_EventsId",
                table: "CategoryEvent");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CategoryEvent",
                table: "CategoryEvent");

            migrationBuilder.RenameTable(
                name: "CategoryEvent",
                newName: "EventCategories");

            migrationBuilder.RenameIndex(
                name: "IX_CategoryEvent_EventsId",
                table: "EventCategories",
                newName: "IX_EventCategories_EventsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventCategories",
                table: "EventCategories",
                columns: new[] { "CategoriesId", "EventsId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_AspNetUsers_BuyerId",
                table: "Bookings",
                column: "BuyerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Events_EventId",
                table: "Bookings",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EventCategories_Categories_CategoriesId",
                table: "EventCategories",
                column: "CategoriesId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EventCategories_Events_EventsId",
                table: "EventCategories",
                column: "EventsId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_AspNetUsers_BuyerId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Events_EventId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_EventCategories_Categories_CategoriesId",
                table: "EventCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_EventCategories_Events_EventsId",
                table: "EventCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EventCategories",
                table: "EventCategories");

            migrationBuilder.RenameTable(
                name: "EventCategories",
                newName: "CategoryEvent");

            migrationBuilder.RenameIndex(
                name: "IX_EventCategories_EventsId",
                table: "CategoryEvent",
                newName: "IX_CategoryEvent_EventsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CategoryEvent",
                table: "CategoryEvent",
                columns: new[] { "CategoriesId", "EventsId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_AspNetUsers_BuyerId",
                table: "Bookings",
                column: "BuyerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Events_EventId",
                table: "Bookings",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryEvent_Categories_CategoriesId",
                table: "CategoryEvent",
                column: "CategoriesId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryEvent_Events_EventsId",
                table: "CategoryEvent",
                column: "EventsId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
