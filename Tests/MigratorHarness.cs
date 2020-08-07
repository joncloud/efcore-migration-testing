using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Threading.Tasks;

namespace Tests
{
    public class MigratorHarness<TContext> : IDisposable
        where TContext : DbContext
    {
        readonly string _connectionString;
        readonly DbContextOptions<TContext> _options;
        readonly TContext _context;
        readonly IMigrator _migrator;
        MigratorHarness()
        {
            var builder = new SqlConnectionStringBuilder
            {
                IntegratedSecurity = true,
                DataSource = "(localdb)\\mssqllocaldb",
                InitialCatalog = Guid.NewGuid().ToString()
            };

            _connectionString = builder.ConnectionString;
            _options = new DbContextOptionsBuilder<TContext>()
                .UseSqlServer(_connectionString)
                .Options;
            
            _context = (TContext)Activator.CreateInstance(typeof(TContext), _options);
            _migrator = _context.GetService<IMigrator>();
        }

        public static async Task<MigratorHarness<TContext>> CreateAsync()
        {
            var harness = new MigratorHarness<TContext>();

            await harness.CreateDatabaseAsync();

            return harness;
        }

        async Task CreateDatabaseAsync()
        {
            // Create the database using a base DbContext
            // to ensure it has the _EFMigrationHistory table.
            var context = new DbContext(
                new DbContextOptionsBuilder()
                    .UseSqlServer(_connectionString)
                    .Options
            );
            await context.Database.EnsureCreatedAsync();
        }

        public async Task UseConnectionAsync(Action<SqlConnection> fn)
        {
            await UseConnectionAsync(conn =>
            {
                fn(conn);
                return Task.FromResult(0);
            });
        }

        public async Task<TResult> UseConnectionAsync<TResult>(Func<SqlConnection, Task<TResult>> fn)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            return await fn(connection);
        }

        public async Task<MigratorHarness<TContext>> TargetMigrationAsync(string migrationName)
        {
            await _migrator.MigrateAsync(migrationName);
            return this;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                _context.Database.EnsureDeleted();

                disposedValue = true;
            }
        }

        ~MigratorHarness()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
