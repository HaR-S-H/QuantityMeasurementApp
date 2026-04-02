using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using QuantityMeasurementApp.Repository;

#nullable disable

namespace QuantityMeasurementApp.Repository.database.Migrations
{
    /// <inheritdoc />
    [DbContext(typeof(QuantityMeasurementDbContext))]
    [Migration("20260401090000_AddOperationUserId")]
    public partial class AddOperationUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                schema: "dbo",
                table: "QuantityMeasurementOperations",
                type: "uniqueidentifier",
                nullable: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_QuantityMeasurementOperations_UserId",
                schema: "dbo",
                table: "QuantityMeasurementOperations",
                column: "UserId"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_QuantityMeasurementOperations_Users_UserId",
                schema: "dbo",
                table: "QuantityMeasurementOperations",
                column: "UserId",
                principalSchema: "dbo",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuantityMeasurementOperations_Users_UserId",
                schema: "dbo",
                table: "QuantityMeasurementOperations"
            );

            migrationBuilder.DropIndex(
                name: "IX_QuantityMeasurementOperations_UserId",
                schema: "dbo",
                table: "QuantityMeasurementOperations"
            );

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "dbo",
                table: "QuantityMeasurementOperations"
            );
        }
    }
}
