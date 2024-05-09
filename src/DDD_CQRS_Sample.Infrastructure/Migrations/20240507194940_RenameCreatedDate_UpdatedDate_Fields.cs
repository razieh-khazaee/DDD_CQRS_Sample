using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DDD_CQRS_Sample.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameCreatedDate_UpdatedDate_Fields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdatedDate",
                table: "Products",
                newName: "UpdatedOn");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "Products",
                newName: "CreatedOn");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdatedOn",
                table: "Products",
                newName: "UpdatedDate");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                table: "Products",
                newName: "CreatedDate");
        }
    }
}
