using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Threading.Tasks;

namespace Tests
{
    public struct SetupMigration<TArrangement>
    {
        readonly Func<SqlConnection, Task<TArrangement>> _fn;
        readonly SourceMigration _sourceMigration;
        public SetupMigration(SourceMigration sourceMigration, Func<SqlConnection, Task<TArrangement>> fn)
        {
            _sourceMigration = sourceMigration;
            _fn = fn;
        }

        public TargetMigration<TArrangement> To<TMigration>() where TMigration : Migration =>
            To(typeof(TMigration).Name);

        public TargetMigration<TArrangement> To(string migrationName) =>
            new TargetMigration<TArrangement>(_sourceMigration, _fn, migrationName);
    }
}
