using System.ComponentModel.DataAnnotations;

namespace ChatPlatform.Server.DTOs
{
    public class CreateGroupRequest
    {
        [Required]
        [MaxLength(30)]
        public string Name { get; set; } = string.Empty;
    }
}
