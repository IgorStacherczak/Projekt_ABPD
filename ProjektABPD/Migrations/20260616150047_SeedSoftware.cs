using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjektABPD.Migrations
{
    /// <inheritdoc />
    public partial class SeedSoftware : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Softwares",
                columns: new[] { "IdSoftware", "Category", "Description", "Name" },
                values: new object[] { 1, "Business", "System do zarządzania klientami", "System CRM" });

            migrationBuilder.InsertData(
                table: "SoftwareVersions",
                columns: new[] { "IdSoftwareVersion", "IdSoftware", "VersionNumber" },
                values: new object[] { 1, 1, "1.0" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SoftwareVersions",
                keyColumn: "IdSoftwareVersion",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Softwares",
                keyColumn: "IdSoftware",
                keyValue: 1);
        }
    }
}
