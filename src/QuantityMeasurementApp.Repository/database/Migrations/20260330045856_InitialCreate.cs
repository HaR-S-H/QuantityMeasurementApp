using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuantityMeasurementApp.Repository.database.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"
                IF SCHEMA_ID(N'dbo') IS NULL
                BEGIN
                    EXEC(N'CREATE SCHEMA [dbo]');
                END

                IF OBJECT_ID(N'[dbo].[QuantityMeasurementOperations]', N'U') IS NULL
                BEGIN
                    CREATE TABLE [dbo].[QuantityMeasurementOperations] (
                        [Id] uniqueidentifier NOT NULL,
                        [Description] nvarchar(1000) NOT NULL,
                        [IsError] bit NOT NULL,
                        [ErrorMessage] nvarchar(1000) NOT NULL,
                        [CreatedAt] datetime2 NOT NULL,
                        CONSTRAINT [PK_QuantityMeasurementOperations] PRIMARY KEY ([Id])
                    );
                END

                IF NOT EXISTS (
                    SELECT 1
                    FROM sys.indexes
                    WHERE [name] = N'IX_QuantityMeasurementOperations_CreatedAt'
                      AND [object_id] = OBJECT_ID(N'[dbo].[QuantityMeasurementOperations]')
                )
                BEGIN
                    CREATE INDEX [IX_QuantityMeasurementOperations_CreatedAt]
                    ON [dbo].[QuantityMeasurementOperations]([CreatedAt]);
                END
                "
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"
                IF OBJECT_ID(N'[dbo].[QuantityMeasurementOperations]', N'U') IS NOT NULL
                BEGIN
                    DROP TABLE [dbo].[QuantityMeasurementOperations];
                END
                "
            );
        }
    }
}
