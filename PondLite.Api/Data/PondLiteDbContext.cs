using Microsoft.EntityFrameworkCore;
using PondLite.Api.Models;

namespace PondLite.Api.Data
{
    public class PondLiteDbContext : DbContext
    {
        public PondLiteDbContext(DbContextOptions<PondLiteDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserAccount> UserAccounts { get; set; }

        public DbSet<AuthToken> AuthTokens { get; set; }
    }
}