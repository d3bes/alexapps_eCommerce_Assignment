﻿using Microsoft.EntityFrameworkCore.Migrations;
using project.Core.Consts;

#nullable disable

namespace project.EF.Migrations
{
    /// <inheritdoc />
    public partial class AddRolesMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql(@$"
            INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp)
            VALUES ('{Boilerplate.boilerplateAdminRoleId}', '{Role.admin}', '{Role.admin.ToUpper()}', NEWID())");

            migrationBuilder.Sql(@$"
            INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp)
            VALUES ('{Boilerplate.boilerplateMerchantRoleId}', '{Role.merchant}', '{Role.merchant.ToUpper()}', NEWID())");



            migrationBuilder.Sql(@$"
            INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp)
            VALUES ('{Boilerplate.boilerplateUserRoleId}', '{Role.user}', '{Role.user.ToUpper()}', NEWID())");


            migrationBuilder.Sql(@$"
            INSERT INTO Users (Id,  FullName, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount)
            VALUES ('{Boilerplate.boilerplateAdminId}', 'Administrator', 'admin', 'ADMIN', 'admin@example.com', 'ADMIN@EXAMPLE.COM', 1, '{Boilerplate.AdminPasswordHash}' , NEWID(), NEWID(), 0, 0, 1, 0)");

         

            // Assign the Admin role to the Admin user
            migrationBuilder.Sql(@$"
            INSERT INTO AspNetUserRoles (UserId, RoleId)
            VALUES ('{Boilerplate.boilerplateAdminId}', '{Boilerplate.boilerplateAdminRoleId}')");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"DELETE FROM AspNetRoles WHERE Id = '{Boilerplate.boilerplateAdminRoleId}'");
            migrationBuilder.Sql($"DELETE FROM AspNetRoles WHERE Id = '{Boilerplate.boilerplateMerchantRoleId}'");
            migrationBuilder.Sql($"DELETE FROM AspNetRoles WHERE Id = '{Boilerplate.boilerplateUserRoleId}'");

            migrationBuilder.Sql($"DELETE FROM AspNetUserRoles WHERE UserId = '{Boilerplate.boilerplateAdminId}' AND RoleId = '{Boilerplate.boilerplateAdminRoleId}'");
            migrationBuilder.Sql($"DELETE FROM Users WHERE Id = '{Boilerplate.boilerplateAdminId}' ");

        }
    }
}