using Microsoft.EntityFrameworkCore;
using SignalRApp.Data.EntityFramework.Configurations;
using SignalRApp.Data.Models;

namespace SignalRApp.Data.EntityFramework
{
    public class SignalRAppDbContext : DbContext
    {
        public SignalRAppDbContext(DbContextOptions<SignalRAppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new ChatRoomConfiguration());
            modelBuilder.ApplyConfiguration(new ChatMessageConfiguration());
            modelBuilder.ApplyConfiguration(new ChatRoomUserConfiguration());
        }

        public DbSet<User> Users { get; set; }
        public DbSet<ChatRoom> ChatRooms { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<ChatRoomUser> ChatRoomUsers { get; set; }
    }
}