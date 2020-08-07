using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Tests
{
    public struct SourceMigration
    {
        readonly string _migrationName;
        public SourceMigration(string migrationName)
        {
            _migrationName = migrationName;
        }

        public SetupMigration<TArrangement> Setup<TArrangement>(Func<SqlConnection, Task<TArrangement>> fn) =>
            new SetupMigration<TArrangement>(this, fn);

        public async Task ApplyAsync<TContext>(MigratorHarness<TContext> harness)
            where TContext : DbContext
        {
            await harness.TargetMigrationAsync(_migrationName);
        }
    }
}
