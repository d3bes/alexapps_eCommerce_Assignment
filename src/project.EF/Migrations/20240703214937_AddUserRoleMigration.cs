using Microsoft.EntityFrameworkCore.Migrations;
using project.Core.Consts;

#nullable disable

namespace project.EF.Migrations
{
    /// <inheritdoc />
    public partial class AddUserRoleMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@$"
            INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp)
            VALUES ('{Boilerplate.boilerplateUserRoleId}', '{Role.user}', '{Role.user.ToUpper()}', NEWID())");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"DELETE FROM AspNetRoles WHERE Id = '{Boilerplate.boilerplateUserRoleId}'");

        }
    }
}
