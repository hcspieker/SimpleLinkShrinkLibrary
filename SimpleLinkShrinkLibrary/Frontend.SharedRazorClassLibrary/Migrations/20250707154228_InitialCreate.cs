using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

// dotnet ef migrations add InitialCreate --project Frontend.SharedRazorClassLibrary --startup-project Frontend.Test.WebExampleAppNet9 --verbose

namespace SimpleLinkShrinkLibrary.Frontend.SharedRazorClassLibrary.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Shortlinks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TargetUrl = table.Column<string>(type: "TEXT", nullable: false),
                    Alias = table.Column<string>(type: "TEXT", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shortlinks", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Shortlinks_Alias",
                table: "Shortlinks",
                column: "Alias",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Shortlinks");
        }
    }
}
