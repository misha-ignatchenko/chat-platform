using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Options;

namespace ChatPlatform.Server.Data
{
    public class ChatPlatformDbContextFactory : IDesignTimeDbContextFactory<ChatPlatformDbContext>
    {
        public ChatPlatformDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ChatPlatformDbContext>();

            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=ChatPlatformDb;Trusted_Connection=True;TrustServerCertificate=True");

            return new ChatPlatformDbContext(optionsBuilder.Options);
        }
    }
}
