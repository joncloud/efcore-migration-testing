using Microsoft.EntityFrameworkCore;

namespace Source
{
    public class SchoolContext : DbContext
    {
        public SchoolContext(DbContextOptions<SchoolContext> options) 
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
    }
}
