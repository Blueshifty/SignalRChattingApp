using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SignalRApp.Business.DTOs.Response;
using SignalRApp.Business.Utilities.Mapper;
using SignalRApp.Data.EntityFramework;
using SignalRApp.Data.Models;

namespace SignalRApp.Business.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly SignalRAppDbContext _context;

        private readonly IMapper _mapper;

        private static readonly List<UserDto> Connections = new List<UserDto>();

        private static readonly List<ChatRoomDto> Rooms = new List<ChatRoomDto>();

        private static readonly Dictionary<string, string> ConnectionMap = new Dictionary<string, string>();


        public ChatHub(SignalRAppDbContext context, IMapper mapper)
        {
            _context = context;

            _mapper = mapper;
        }


        public async Task SendToRoom(string roomName, string message)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.UserName == IdentityName);
                var room = _context.ChatRooms.FirstOrDefault(r => r.Name == roomName);

                if (!string.IsNullOrEmpty(message.Trim()))
                {
                    var chatMessage = new ChatMessage
                    {
                        Message = Regex.Replace(message, @"(?i)<(?!img|a|/a|/img).*?>", string.Empty),
                        Author = user,
                        ChatRoom = room,
                    };
                    _context.ChatMessages.Add(chatMessage);
                    _context.SaveChanges();
                    var messageDto = _mapper.Map<ChatMessage, ChatMessageDto>(chatMessage);
                    await Clients.Group(roomName).SendAsync("newMessage", messageDto);
                }
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("onError", "Error : " + ex.Message);
            }
        }

        public async Task Join(string roomName)
        {
            try
            {
                var user = Connections.FirstOrDefault(u => u.UserName == IdentityName);
                if (user != null && user.CurrentRoom != roomName)
                {
                    if (!string.IsNullOrEmpty(user.CurrentRoom))
                    {
                        await Clients.OthersInGroup(user.CurrentRoom).SendAsync("removeUser", user);
                    }

                    if (user.CurrentRoom != null)
                    {
                        await Leave(user.CurrentRoom);
                    }

                    await Groups.AddToGroupAsync(Context.ConnectionId, roomName);

                    user.CurrentRoom = roomName;

                    await Clients.OthersInGroup(roomName).SendAsync("addUser", user);
                }
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("onError", "You failed to join the chat room!" + ex.Message);
            }
        }

        public async Task Leave(string roomName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
        }

        public async Task CreateRoom(string roomName)
        {
            try
            {
                var match = Regex.Match(roomName, @"^\w+( \w+)*$");
                if (!match.Success)
                {
                    await Clients.Caller.SendAsync("onError",
                        "Invalid room name, room name must contain only letters and numbers");
                }
                else if (roomName.Length < 5 || roomName.Length > 100)
                {
                    await Clients.Caller.SendAsync("onError",
                        "Room name must be between 5-100 characters!");
                }
                else if (_context.ChatRooms.Any(r => r.Name == roomName))
                {
                    await Clients.Caller.SendAsync("onError", "Another chat room with this name exists");
                }
                else
                {
                    var user = _context.Users.FirstOrDefault(u => u.UserName == IdentityName);
                    var room = new ChatRoom()
                    {
                        Name = roomName,
                    };
                    _context.ChatRooms.Add(room);
                    _context.SaveChanges();

                    var hubRoom = _mapper.Map<ChatRoom, ChatRoomDto>(room);
                    Rooms.Add(hubRoom);
                    await Clients.All.SendAsync("addChatRoom", hubRoom);
                }
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("onError", "Couldn't create chat room: " + ex.Message);
            }
        }

        public IEnumerable<ChatRoomDto> GetRooms()
        {
            // On the first time
            if (Rooms.Count == 0)
            {
                foreach (var room in _context.ChatRooms)
                {
                    var hubRoom = _mapper.Map<ChatRoom, ChatRoomDto>(room);
                    Rooms.Add(hubRoom);
                }
            }

            return Rooms;
        }

        public override Task OnConnectedAsync()
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.UserName == IdentityName);

                var userModel = _mapper.Map<User, UserDto>(user);

                if (Connections.All(u => u.UserName != IdentityName))
                {
                    Connections.Add(userModel);
                    ConnectionMap.Add(IdentityName, Context.ConnectionId);
                }

                Clients.Caller.SendAsync("getRooms", userModel.UserName);
            }
            catch (Exception ex)
            {
                Clients.Caller.SendAsync("onError", "OnConnected" + ex.Message);
            }

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            try
            {
                var user = Connections.First(u => u.UserName == IdentityName);

                Clients.OthersInGroup(user.CurrentRoom).SendAsync("removeUser", user);

                ConnectionMap.Remove(user.UserName);
            }
            catch (Exception ex)
            {
                Clients.Caller.SendAsync("onError", "OnDisconnected: " + ex.Message);
            }

            return base.OnDisconnectedAsync(exception);
        }


        public IEnumerable<UserDto> GetOnlineUsersInRoom(string roomName)
        {
            return Connections.Where(u => u.CurrentRoom == roomName).ToList();
        }

        public IEnumerable<ChatMessageDto> GetMessageHistory(string roomName)
        {
            var messageHistory = _context.ChatMessages.Where(m => m.ChatRoom.Name == roomName)
                .Include(m => m.Author)
                .OrderByDescending(m => m.CreatedAt)
                .ToList();

            return _mapper.Map<IEnumerable<ChatMessage>, IEnumerable<ChatMessageDto>>(messageHistory);
        }

        private string IdentityName => Context.User.Identity.Name;

        private string GetDevice()
        {
            var device = Context.GetHttpContext().Request.Headers["Device"].ToString();
            if (!string.IsNullOrEmpty(device) && (device.Equals("Desktop") || device.Equals("Mobile")))
                return device;

            return "Web";
        }
    }
}