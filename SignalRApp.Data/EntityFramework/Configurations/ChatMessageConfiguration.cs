using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalRApp.Data.Models;

namespace SignalRApp.Data.EntityFramework.Configurations
{
    public class ChatMessageConfiguration : IEntityTypeConfiguration<ChatMessage>
    {
        public void Configure(EntityTypeBuilder<ChatMessage> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.AuthorId).IsRequired();
            builder.Property(x => x.Message).IsRequired().HasMaxLength(256);
            builder.Property(x => x.ChatRoomId).IsRequired();
            builder.Property(x => x.CreatedAt).IsRequired();
        }
    }
}