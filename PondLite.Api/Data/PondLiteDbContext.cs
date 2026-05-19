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

        public DbSet<DailyCheckIn> DailyCheckIns { get; set; }

        public DbSet<Ribbit> Ribbits { get; set; }

        public DbSet<DiscussionPrompt> DiscussionPrompts { get; set; }

        public DbSet<DiscussionResponse> DiscussionResponses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // - - -AuthToken- - -
            modelBuilder.Entity<AuthToken>()
                .HasOne(token => token.UserAccount)
                .WithMany()
                .HasForeignKey(token => token.UserAccountId)
                .OnDelete(DeleteBehavior.Cascade);

            // - - -UserAccount- - -
            modelBuilder.Entity<UserAccount>()
                .Property(user => user.AccountType)
                .HasConversion<string>();

            // - - -RelationshipMember- - -
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

            // - - -Frog- - -
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

            // - - -DailyCheckIn- - -
            modelBuilder.Entity<DailyCheckIn>()
                .HasOne(checkIn => checkIn.Relationship)
                .WithMany()
                .HasForeignKey(checkIn => checkIn.RelationshipId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DailyCheckIn>()
                .HasOne(checkIn => checkIn.UserAccount)
                .WithMany()
                .HasForeignKey(checkIn => checkIn.UserAccountId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DailyCheckIn>()
                .HasIndex(checkIn => new
                {
                    checkIn.RelationshipId,
                    checkIn.UserAccountId,
                    checkIn.CheckInDate
                })
                .IsUnique();

            modelBuilder.Entity<DailyCheckIn>()
                .Property(checkIn => checkIn.PrimaryEmotion)
                .HasConversion<string>();

            modelBuilder.Entity<DailyCheckIn>()
                .Property(checkIn => checkIn.SecondaryEmotion)
                .HasConversion<string>();

            modelBuilder.Entity<DailyCheckIn>()
                .Property(checkIn => checkIn.TertiaryEmotion)
                .HasConversion<string>();

            // - - -Ribbit- - -
            modelBuilder.Entity<Ribbit>()
                .HasOne(ribbit => ribbit.Relationship)
                .WithMany()
                .HasForeignKey(ribbit => ribbit.RelationshipId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Ribbit>()
                .HasOne(ribbit => ribbit.SenderUser)
                .WithMany()
                .HasForeignKey(ribbit => ribbit.SenderUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Ribbit>()
                .HasOne(ribbit => ribbit.ReceiverUser)
                .WithMany()
                .HasForeignKey(ribbit => ribbit.ReceiverUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Ribbit>()
                .Property(ribbit => ribbit.Type)
                .HasConversion<string>();

            modelBuilder.Entity<Ribbit>()
                .Property(ribbit => ribbit.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Ribbit>()
                .HasIndex(ribbit => new
                {
                    ribbit.RelationshipId,
                    ribbit.ReceiverUserId,
                    ribbit.Status
                });

            modelBuilder.Entity<Ribbit>()
                .HasIndex(ribbit => new
                {
                    ribbit.RelationshipId,
                    ribbit.SenderUserId,
                    ribbit.Status
                });

            // - - -DiscussionPrompt- - -
            modelBuilder.Entity<DiscussionPrompt>()
                .HasOne(prompt => prompt.Relationship)
                .WithMany()
                .HasForeignKey(prompt => prompt.RelationshipId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DiscussionPrompt>()
                .HasIndex(prompt => new
                {
                    prompt.RelationshipId,
                    prompt.PromptDate
                })
                .IsUnique();

            // - - -DiscussionResponse- - -
            modelBuilder.Entity<DiscussionResponse>()
                .HasOne(response => response.DiscussionPrompt)
                .WithMany(prompt => prompt.Responses)
                .HasForeignKey(response => response.DiscussionPromptId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DiscussionResponse>()
                .HasOne(response => response.UserAccount)
                .WithMany()
                .HasForeignKey(response => response.UserAccountId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DiscussionResponse>()
                .HasIndex(response => new
                {
                    response.DiscussionPromptId,
                    response.UserAccountId
                })
                .IsUnique();
        }
    }
}
