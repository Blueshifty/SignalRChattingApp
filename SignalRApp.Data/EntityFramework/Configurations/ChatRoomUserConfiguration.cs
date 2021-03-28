using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalRApp.Data.Models;

namespace SignalRApp.Data.EntityFramework.Configurations
{
    public class ChatRoomUserConfiguration : IEntityTypeConfiguration<ChatRoomUser>
    {
        public void Configure(EntityTypeBuilder<ChatRoomUser> builder)
        {
            builder.HasKey(x => new {x.ChatRoomId, x.UserId});

            builder.HasOne(x => x.ChatRoom)
                .WithMany(x => x.Users)
                .HasForeignKey(x => x.ChatRoomId);

            builder.HasOne(x => x.User)
                .WithMany(x => x.ChatRooms)
                .HasForeignKey(x => x.UserId);
        }
    }
}