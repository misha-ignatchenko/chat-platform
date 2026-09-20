using ChatPlatform.Server.Entities;
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
            modelBuilder.Entity<GroupMember>()
                .HasKey(gm => new { gm.UserId, gm.GroupId });
        }
    }
}
