using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Source
{
    public class SchoolContextFactory : IDesignTimeDbContextFactory<SchoolContext>
    {
        public SchoolContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SchoolContext>();

            var builder = new SqlConnectionStringBuilder
            {
                IntegratedSecurity = true,
                DataSource = "(localdb)\\mssqllocaldb",
                InitialCatalog = "SchoolContext"
            };

            optionsBuilder.UseSqlServer(builder.ConnectionString);

            return new SchoolContext(optionsBuilder.Options);
        }
    }
}
