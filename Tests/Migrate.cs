using Microsoft.EntityFrameworkCore.Migrations;

namespace Tests
{
    public static class Migrate
    {
        public static SourceMigration From(string migrationName) =>
            new SourceMigration(migrationName);

        public static SourceMigration From<T>() where T : Migration =>
            From(typeof(T).Name);
    }
}
