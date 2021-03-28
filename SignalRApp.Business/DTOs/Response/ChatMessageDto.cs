using System;

namespace SignalRApp.Business.DTOs.Response
{
    public class ChatMessageDto
    {
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public UserInChatMessageDto Author { get; set; }
    }

    public class UserInChatMessageDto
    {
        public string UserName { get; set; }
    }
}