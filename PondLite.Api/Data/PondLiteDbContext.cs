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

        public DbSet<Relationship> Relationships { get; set; }

        public DbSet<RelationshipMember> RelationshipMembers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AuthToken>()
                .HasOne(token => token.UserAccount)
                .WithMany()
                .HasForeignKey(token => token.UserAccountId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RelationshipMember>()
                .HasOne(member => member.UserAccount)
                .WithMany(user => user.RelationshipMembers)
                .HasForeignKey(member => member.UserAccountId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RelationshipMember>()
                .HasOne(member => member.Relationship)
                .WithMany(relationship => relationship.Members)
                .HasForeignKey(member => member.RelationshipId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RelationshipMember>()
                .HasIndex(member => new { member.RelationshipId, member.UserAccountId })
                .IsUnique();
        }
    }
}