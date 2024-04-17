using Microsoft.EntityFrameworkCore;

namespace AuthenticationAPI.Database
{
    public class DatabaseContext : DbContext
    {
        
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<Users> Users { get; set; }
        public DbSet<SessionTokens> SessionTokens { get; set; }
        public DbSet<ConfirmationCodes> ConfirmationCodes { get; set; }
    }
}
