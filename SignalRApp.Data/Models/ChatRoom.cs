using System.Collections.Generic;

namespace SignalRApp.Data.Models
{
    public class ChatRoom : BaseModel
    {
        public string Name { get; set; }
        public IList<ChatRoomUser> Users { get; set; }
        public IList<ChatMessage> Messages { get; set; }
    }
}