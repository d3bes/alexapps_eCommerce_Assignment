using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace project.EF.Migrations
{
    /// <inheritdoc />
    public partial class AdminMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Insert the Admin role
            migrationBuilder.Sql(@"
            INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp)
            VALUES (' 72ede098-49aa-4056-8912-63b605e52740', 'Admin', 'ADMIN', NEWID())");

            // Insert the Admin user
            migrationBuilder.Sql(@"
            INSERT INTO AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount)
            VALUES ('92c8a8b2-647f-456f-a787-20e396c8bc84', 'admin', 'ADMIN', 'admin@example.com', 'ADMIN@EXAMPLE.COM', 1, 'AQAAAAIAAYagAAAAEB9Egl04rGcrKfVSsFecpExW/EZdaGCs4hZkzeecpd6mP9ntxAu4fOEJmDGfQQZpgQ==', NEWID(), NEWID(), 0, 0, 1, 0)");

            // Assign the Admin role to the Admin user
            migrationBuilder.Sql(@"
            INSERT INTO AspNetUserRoles (UserId, RoleId)
            VALUES ('92c8a8b2-647f-456f-a787-20e396c8bc84', '72ede098-49aa-4056-8912-63b605e52740')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM AspNetUserRoles WHERE UserId = '92c8a8b2-647f-456f-a787-20e396c8bc84' AND RoleId = '72ede098-49aa-4056-8912-63b605e52740'");
            migrationBuilder.Sql("DELETE FROM AspNetUsers WHERE Id = '92c8a8b2-647f-456f-a787-20e396c8bc84'");
            migrationBuilder.Sql("DELETE FROM AspNetRoles WHERE Id = '72ede098-49aa-4056-8912-63b605e52740'");
        }
    }
}
