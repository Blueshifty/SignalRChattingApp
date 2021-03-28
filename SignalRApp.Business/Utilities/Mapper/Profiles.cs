using SignalRApp.Business.DTOs.Request;
using SignalRApp.Business.DTOs.Response;
using SignalRApp.Data.Models;

namespace SignalRApp.Business.Utilities.Mapper
{
    public class Profiles : AutoMapper.Profile
    {
        public Profiles()
        {
            CreateMap<RegisterDto, User>();
            CreateMap<ChatMessage, ChatMessageDto>().ForMember(x => x.Author, o => o.MapFrom(x => x.Author));
            CreateMap<ChatRoom, ChatRoomDto>();
            CreateMap<User, UserDto>();
            CreateMap<User, UserInChatMessageDto>();
        }
    }
}