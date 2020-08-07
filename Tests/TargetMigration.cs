using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Source;
using System;
using System.Threading.Tasks;

namespace Tests
{
    public struct TargetMigration<TArrangement>
    {
        readonly string _migrationName;
        readonly Func<SqlConnection, Task<TArrangement>> _fn;
        readonly SourceMigration _sourceMigration;
        public TargetMigration(SourceMigration sourceMigration, Func<SqlConnection, Task<TArrangement>> fn, string migrationName)
        {
            _sourceMigration = sourceMigration;
            _fn = fn;
            _migrationName = migrationName;
        }

        public async Task AssertAsync<TContext>(Func<TArrangement, SqlConnection, Task> fn)
            where TContext : DbContext
        {
            using var harness = await MigratorHarness<TContext>.CreateAsync();

            await _sourceMigration.ApplyAsync(harness);

            TArrangement arrangement = await harness.UseConnectionAsync(_fn);

            await harness.TargetMigrationAsync(_migrationName);

            await harness.UseConnectionAsync(conn => fn(arrangement, conn));
        }
    }
}
