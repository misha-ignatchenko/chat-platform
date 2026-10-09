using ChatPlatform.Server.Data;
using ChatPlatform.Server.Entities;
using ChatPlatform.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ChatPlatform.Server.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly ChatPlatformDbContext _context;
        public override async Task OnConnectedAsync()
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userName = Context.User?.FindFirst(ClaimTypes.Name)?.Value;

            await base.OnConnectedAsync();
        }

        public async Task SendMessage(Guid groupId, string text)
        {
            var userIdNullable = GetUserId();

            if (userIdNullable is not Guid userId)
            {
                return;
            }

            var isMember = await IsMemberAsync(userId, groupId);

            if (!isMember)
                throw new HubException("You are not a member of this group");

            Message message = new Message(userId,groupId,text);
            _context.Messages.Add(message);

            await _context.SaveChangesAsync();

            var chatMessage = new ChatMessage(message.Id, message.SenderId, message.GroupId, message.Text, message.SentAt);
            await Clients.Group(groupId.ToString()).SendAsync("ReceiveMessage", chatMessage);
        }

        public async Task JoinGroup(Guid groupId)
        {
            var userIdNullable = GetUserId();

            if (userIdNullable is not Guid userId)
            {
                return;
            }

            var isMember = await IsMemberAsync(userId, groupId);

            if (!isMember)
                throw new HubException("You are not a member of this group");

            await Groups.AddToGroupAsync(Context.ConnectionId, groupId.ToString());
        }

        private Guid? GetUserId()
        {
            var userIdString = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdString == null || !Guid.TryParse(userIdString, out var userId))
                return null;

            return userId;
        }

        private async Task<bool> IsMemberAsync(Guid userId, Guid groupId)
        {
            return await _context.GroupMembers.AnyAsync(gm => gm.UserId == userId && gm.GroupId == groupId);
        }
    }
}
