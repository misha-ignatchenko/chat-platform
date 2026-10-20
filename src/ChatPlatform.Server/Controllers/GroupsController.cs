using ChatPlatform.Server.Data;
using ChatPlatform.Server.DTOs;
using ChatPlatform.Server.Entities;
using ChatPlatform.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

namespace ChatPlatform.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class GroupsController : ControllerBase
    {
        private readonly ChatPlatformDbContext _context;

        public GroupsController(ChatPlatformDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateGroupRequest request)
        {
            var userIdNullable = GetUserId();

            if (userIdNullable is not Guid userId)
            {
                return Unauthorized();
            }

            Group newGroup = new Group(request.Name, userId);
            GroupMember newMember = new GroupMember(userId,newGroup.Id,GroupRole.Owner);

            _context.Groups.Add(newGroup);
            _context.GroupMembers.Add(newMember);

            await _context.SaveChangesAsync();

            return Created($"/api/groups/{newGroup.Id}", new { Id = newGroup.Id });
        }

        [HttpPost("{groupId:guid}/members")]
        public async Task<IActionResult> AddMember(Guid groupId, [FromBody] AddMemberRequest request)
        {
            var userIdNullable = GetUserId();

            if (userIdNullable is not Guid userId)
            {
                return Unauthorized();
            }

            var isMember = await _context.GroupMembers.AnyAsync(gm => gm.UserId == userId && gm.GroupId == groupId);
            if(!isMember)
                return Forbid();

            var userToAdd = await _context.Users.FirstOrDefaultAsync(u => u.Login == request.Login);
            if (userToAdd == null)
                return NotFound("User not found");

            var isUserMember = await _context.GroupMembers.AnyAsync(gm => gm.UserId == userToAdd.Id && gm.GroupId == groupId);
            if(isUserMember)
                return Conflict("User is already a member");

            GroupMember newMember = new GroupMember(userToAdd.Id, groupId, GroupRole.Member);
            _context.GroupMembers.Add(newMember);

            await _context.SaveChangesAsync();

            return Ok();
        }

        private Guid? GetUserId()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdString == null || !Guid.TryParse(userIdString, out var userId))
                return null;

            return userId;
        }
    }
}
