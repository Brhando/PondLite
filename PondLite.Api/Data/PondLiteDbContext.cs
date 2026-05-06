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

        public DbSet<Frog> Frogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AuthToken>()
                .HasOne(token => token.UserAccount)
                .WithMany()
                .HasForeignKey(token => token.UserAccountId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserAccount>()
                .Property(user => user.AccountType)
                .HasConversion<string>();

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

            modelBuilder.Entity<Frog>()
                .HasOne(frog => frog.RelationshipMember)
                .WithOne(member => member.Frog)
                .HasForeignKey<Frog>(frog => frog.RelationshipMemberId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Frog>()
                .HasIndex(frog => frog.RelationshipMemberId)
                .IsUnique();

            modelBuilder.Entity<Frog>()
                .Property(frog => frog.CurrentMood)
                .HasConversion<string>();

            modelBuilder.Entity<Frog>()
                .Property(frog => frog.ActivityState)
                .HasConversion<string>();
        }
    }
}
