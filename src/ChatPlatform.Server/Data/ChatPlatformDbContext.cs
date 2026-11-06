using ChatPlatform.Server.Entities;
using ChatPlatform.Shared;
using Microsoft.EntityFrameworkCore;

namespace ChatPlatform.Server.Data
{
    public class ChatPlatformDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupMember> GroupMembers { get; set; }
        public DbSet<Message> Messages { get; set; }

        public ChatPlatformDbContext(DbContextOptions<ChatPlatformDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GroupMember>(entity =>
            {
                entity.HasKey(gm => new { gm.UserId, gm.GroupId });

                entity.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(gm => gm.UserId);

                entity.HasOne<Group>()
                    .WithMany()
                    .HasForeignKey(gm => gm.GroupId);
            });

            modelBuilder.Entity<Group>(entity =>
            {
                entity.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(g => g.OwnerId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Message>()
                .Property(m => m.Text)
                .HasMaxLength(MessageLimits.MaxTextLength);

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(u => u.Login).HasMaxLength(UserLimits.MaxLoginLength);
                entity.HasIndex(u => u.Login).IsUnique();
            });
        }
    }
}
