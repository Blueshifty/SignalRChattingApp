using System;

namespace SignalRApp.Data.Models
{
    public class ChatMessage : BaseModel
    {
        public ChatMessage()
        {
            CreatedAt = DateTime.Now;
        }

        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public int AuthorId { get; set; }
        public User Author { get; set; }
        public int ChatRoomId { get; set; }
        public ChatRoom ChatRoom { get; set; }
    }
}