using ChatPlatform.Shared;

namespace ChatPlatform.Server.Entities
{
    public class GroupMember
    {
        public Guid UserId { get; set; }
        public Guid GroupId { get; set; }
        public GroupRole Role { get; set; }
        public DateTimeOffset JoinedAt { get; set; }

        public GroupMember(Guid userId, Guid groupId, GroupRole role = GroupRole.Member)
        {
            UserId = userId;
            GroupId = groupId;
            Role = role;
            JoinedAt = DateTimeOffset.UtcNow;
        }
    }
}
