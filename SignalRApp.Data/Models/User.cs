using System.Collections.Generic;

namespace SignalRApp.Data.Models
{
    public class User : BaseModel
    {
        public string UserName { get; set; }
        public byte[] PasswordSalt { get; set; }
        public byte[] PasswordHash { get; set; }

        public string RefreshToken { get; set; }
        public IList<ChatRoomUser> ChatRooms { get; set; }
        public IList<ChatMessage> Messages { get; set; }
    }
}